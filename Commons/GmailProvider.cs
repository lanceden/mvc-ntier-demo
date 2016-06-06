namespace Commons
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// Gmail送信類
    /// </summary>
    public static class GmailProvider
    {
        private static readonly string MailSenderAccount = GetAppsettingValueByName("MailSenderAccount");
        private static readonly string MailSenderPassword = GetAppsettingValueByName("MailSenderPassword");
        private static readonly string ErrorFilePath = GetAppsettingValueByName("ErrorFilePath");
        #region 送出電子郵件
        /// <summary>
        /// 送出電子郵件
        /// </summary>
        /// <param name="content">審核未通過內容</param>
        /// <param name="sendType">送信類別 1:審核通過信 2:審核未通過信</param>
        /// <param name="receiveName">收信人名稱</param>
        /// <param name="receiveMail">收信人</param>
        /// <param name="videoFileCustomName">作品名稱</param>
        public static bool SendMail(string content, int sendType,string receiveName,string receiveMail,string videoFileCustomName)
        {
            try
            {
                //1.0 創建Smtp伺服器
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                //3.0 創建信件內容
                var mailMessage = new MailMessage
                {
                    //3.1 設定信件主旨編碼格式
                    SubjectEncoding = Encoding.UTF8,
                    //3.2 設定信件內容編碼格式
                    BodyEncoding = Encoding.UTF8,
                    //3.3 設定送件人
                    From = new MailAddress(MailSenderAccount, "霹靂無敵活動")
                };
                //3.4 設定收件人
                mailMessage.To.Add(receiveMail);
                //3.5 設定信件主旨
                mailMessage.Subject = "霹靂無敵‧創意無敵影片大賽";
                var sb = new StringBuilder(2000);
                switch (sendType)
                {
                    case 1:
                        sb.Append("親愛的(");
                        sb.Append(receiveName);
                        sb.Append(")您好");
                        sb.AppendLine();
                        sb.Append("感謝您報名《霹靂無敵‧創意無敵影片大賽》，活動小組已收到您的作品(");
                        sb.Append(videoFileCustomName);
                        sb.Append(")，並且已審核通過，人氣票選與官方評選時間為06/01~06/12，您可邀請親朋好友們來一同共襄盛舉，並預祝您獲得活動最高榮譽!");
                        sb.AppendLine();
                        sb.Append("《霹靂無敵‧創意無敵影片大賽》活動小組敬上");
                        break;
                    case 2:
                        sb.Append("親愛的(");
                        sb.Append(receiveName);
                        sb.Append(")您好");
                        sb.AppendLine();
                        sb.Append("感謝您報名《霹靂無敵‧創意無敵影片大賽》，活動小組已收到您的作品(");
                        sb.Append(videoFileCustomName);
                        sb.AppendLine(")，但您的作品因(");
                        sb.Append(content);
                        sb.Append(") 而審核未通過，煩請依據修改後再次報名並重新上傳影片，已完成活動報名。希望能再次看到您優秀的作品!");
                        sb.AppendLine();
                        sb.Append("《霹靂無敵‧創意無敵影片大賽》活動小組敬上");
                        break;
                }
                //3.6 設定信件內容
                mailMessage.Body = sb.ToString();
                //4.0 設定寄件者的帳號及密碼
                smtpClient.Credentials = new NetworkCredential(MailSenderAccount, MailSenderPassword);
                //5.0 送出信件
                smtpClient.ServicePoint.MaxIdleTime = 1;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                using (var sw = new System.IO.StreamWriter(ErrorFilePath, true)) sw.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion
        #region 獲取Appsetting的字符串
        /// <summary>
        /// 獲取Appsetting的字符串
        /// </summary>
        /// <param name="appsettingName">Appsetting定義的名稱</param>
        /// <returns>字符串</returns>
        private static string GetAppsettingValueByName(string appsettingName)
        { 
            return ConfigurationManager.AppSettings[appsettingName];
        } 
        #endregion
    }
}
