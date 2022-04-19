namespace bpmNotify
{
    using ezAcquire.Server.Extension.Utility;
    using RestSharp;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    class Program
    {
        static string _hostUrl = System.Configuration.ConfigurationManager.AppSettings["hostUrl"];
        public　static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        public　static void byteArrayToImage(byte[] source)
        {
            MemoryStream ms = new MemoryStream(source);
            Image ret = Image.FromStream(ms);
            ret.Save("test.tif");
        }


        static void Main(string[] args)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(@"./96057080_23895858_A010000__001.tif");
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);



            var tt = new Aes256Crypto("23895858");
            var rr = tt.DecryptString(base64ImageRepresentation);
            var gg = tt.DecryptByteArray(imageArray);
            byteArrayToImage(gg);

            //System.IO.File.WriteAllText(@"C:\Users\ek1008\source\repos\bpmRecognize\bpmNotify\bin\Debug\test.txt", "test");
            // 讀取XML傳送Api資料
            var doc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            doc.Load(@"C:\Users\ek1008\source\repos\bpmRecognize\bpmNotify\bin\Debug\transData.xml");
            //驗證BPM服務是否正常
            var data = VerifyBpmService(doc.InnerXml);
            // 解析Api回傳資料
            var responseData = DeserializeXmlData<Envelope>(data.ResponseData);
            try
            {
                if (responseData == null) throw new NullReferenceException(data.ResponseData);
                // Api有正常回傳Response Code 200，不做額外處理
                if (responseData.Body.getRecognizeResponse.resultCode.Equals("200"))
                {
                    Console.WriteLine($@"getRecognizeResponse.resultCode:{responseData.Body.getRecognizeResponse.resultCode}\r\n
                    ReadWriteTimeout: {data.ReadWriteTimeout}
                    ");
                    System.IO.File.WriteAllText(@"C:\Users\ek1008\source\repos\bpmRecognize\bpmNotify\bin\Debug\test.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    return;
                }
                // Api異常，發送Line Notify通知
                SendLineNotify(data.ResponseData, "Ufce36a58cec02402e2de75a78df398b1");//羽翔
                SendLineNotify(data.ResponseData, "Uf208947c104b105f7de2f9a701d19b69");//Ruska
            }
            catch (Exception ex)
            {
                // Api異常，發送Line Notify通知
                SendLineNotify($"{data.ResponseData} 錯誤訊息:{ex}", "Ufce36a58cec02402e2de75a78df398b1");//羽翔
                SendLineNotify($"{data.ResponseData} 錯誤訊息:{ex}", "Uf208947c104b105f7de2f9a701d19b69");//Ruska
                return;
            }
        }

        /// <summary>
        /// 驗證BPM服務是否正常
        /// </summary>
        /// <param name="transData">Api接收資料</param>
        static ResultModel VerifyBpmService(string transData)
        {
            var _ResultModel = new ResultModel { };
            try
            {
                // 設置Api網址
                var client = new RestClient($"{_hostUrl}") { Timeout = 2000 };
                // 設置傳送請求方式(Get或Post)
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/xml");
                request.AddParameter("application/xml", transData, ParameterType.RequestBody);
                // 發送請求
                var response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _ResultModel.ResponseData = response.ErrorMessage;
                }
                else
                {
                    _ResultModel.ResponseData = response.Content;
                    _ResultModel.ReadWriteTimeout = request.ReadWriteTimeout > 0
                    ? request.ReadWriteTimeout
                    : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _ResultModel;
        }
        // 反序列化XMLsnm psj 
        static T DeserializeXmlData<T>(string s)
        {
            XmlDocument xdoc = new XmlDocument();

            try
            {
                xdoc.LoadXml(s);
                XmlNodeReader reader = new XmlNodeReader(xdoc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(typeof(T));
                object obj = ser.Deserialize(reader);

                return (T)obj;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// /發送Line通知
        /// </summary>
        /// <param name="message">發送訊息</param>
        static void SendLineNotify(string message, string notifyId)
        {
            var client = new RestClient("https://jxjasper.com/JasperLineOfficial/api/LineNotify/notify");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("Message", $"{DateTime.Now} bpm-recognize錯誤:{message}");
            request.AddParameter("stickerPackageId", "6136");
            request.AddParameter("stickerId", "10551377");
            request.AddParameter("NotifyLineId", notifyId);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }

}
