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
            ret.Save("test33.tif");
        }

        public static void byteArrayToFile(byte[] source)
        {
            MemoryStream ms = new MemoryStream(source);
            System.IO.File.WriteAllBytes("ect.txt", source);
        }

        public static void byteArrayToZip(byte[] source)
        {
            MemoryStream ms = new MemoryStream(source);
            System.IO.File.WriteAllBytes("ect.zip", source);
        }

        static void Main(string[] args)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(@"./ect.txt");
            //string base64ImageRepresentation = Convert.ToBase64String(imageArray);



            var tt = new Aes256Crypto("23895858");
            //var ss = tt.EncryptByteArray(imageArray);
            //byteArrayToFile(ss);
            var rr = tt.DecryptByteArray(imageArray);
            byteArrayToZip(rr);

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

/*
目前剩下的那個問題是 發生在 使用者假設輸入了10張照片要比對 如果第二張照片不符合規則(例如:給了空白照片)  那就直接ERROR 後面的照片就全部不比對了
所以要再弄個流程控制~
您已經成功地從 林承諺 收到 SignatureVerification.7z。檔案位於 C:\Users\ek1008\Documents\我已接收的檔案。強烈建議您先用防毒軟體掃描一下，再開啟這個檔案。
林承諺 下午 02:25
他的錯誤發生位置在 src/detection/loader.py 第88行 傳遞到 src/service/instances.py 第182行 再傳到 app.py 第211行 
鄧羽翔 下午 02:26
我看一下
林承諺 下午 02:30
loader.py簡單講就是 他會計算 模型抓的位置是文字的機率為多少 並取出最高的機率 並還原出張量(照片位置) 最後將照片的四個角的座標return出來
林承諺 下午 02:32
instances.py就會去把return直接起來 並丟到簽名辨識的model去執行 然而前一顆model回傳的資料為空值 根本丟不進去第二顆model 所以產生ERROR

*/
