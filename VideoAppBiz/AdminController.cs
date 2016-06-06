using System;
using System.Data.SqlClient;

namespace VideoApp.Biz
{
    using IServices;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web.Mvc;
    public class AdminController:JsonController
    {
        readonly string adminUrl = System.Configuration.ConfigurationManager.AppSettings["AdminUrl"];
        readonly string adminLoginUrl = System.Configuration.ConfigurationManager.AppSettings["AdminLoginUrl"];
        private Ipli_loginAccountService _pli_loginAccountService;
        private Ipli_formDataService _pli_formDataService;
        private Ipli_mailLogService _pli_MailLogService;
        public AdminController(Ipli_loginAccountService pli_loginAccountService,
            Ipli_formDataService pli_formDataService,
            Ipli_mailLogService pli_MailLogService)
        {
            this._pli_loginAccountService = pli_loginAccountService;
            this._pli_formDataService = pli_formDataService;
            this._pli_MailLogService = pli_MailLogService;
        }
        #region 1.0 登入頁面 Get + ActionResult Login()
        /// <summary>
        /// 1.0 登入頁面 Get + ActionResult Login()
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        #endregion
        #region 2.0 登入頁面 Post + ActionResult Login()
        /// <summary>
        /// 2.0 登入頁面 Post + ActionResult Login()
        /// </summary>
        /// <param name="txtName">登入姓名</param>
        /// <param name="txtPassword">登入密碼</param>
        /// <returns>登入是否成功</returns>
        [HttpPost]
        public ActionResult Login(string txtName, string txtPassword)
        {
            var uName = Request.Form["txtName"];
            var uPwd = Request.Form["txtPassword"];
            var res = _pli_loginAccountService.RunSql<byte[]>("select pli_LonginPassword from pli_loginAccount where pli_LonginAccount='" + uName + "'").FirstOrDefault();
            if (res == null || res.Length <= 0) return WriteJsonErr("帳號密碼不正確");
            var salt = System.Configuration.ConfigurationManager.AppSettings["PWSalt"];
            var loginPwd = new SHA1Managed().ComputeHash(System.Text.Encoding.Unicode.GetBytes(uPwd + salt));
            var flag = true;
            for (int i = 0; i < loginPwd.Length; i++)
            {
                if (!loginPwd[i].Equals(res[i]))
                {
                    flag = false;
                    break;
                }
            }
            if (res == null || res.Length <= 0) return WriteJsonErr("帳號密碼不正確");
            var loginModel = _pli_loginAccountService.GetList(s => s.pli_LonginAccount == uName).FirstOrDefault();
            //Session.Add("menuList", GetMenuList(loginModel));
            Session.Add("admin", loginModel);
            return WriteJsonOk("登入成功", adminUrl);
        }
        #endregion
        #region 3.0 登出後臺頁面 + ActionResult Logout()
        /// <summary>
        /// 3.0 登出後臺頁面 + ActionResult Logout()
        /// </summary>
        /// <returns></returns>
        [Admin]
        public ActionResult Logout()
        {
            Session.Remove("admin");
            return Content("<script>alert('您已登出。'); window.location.href = '"+ adminLoginUrl + "';</script>");
        }
        #endregion
        #region 4.0 後臺頁面首頁 + ActionResult Index()
        /// <summary>
        /// 4.0 後臺頁面首頁 + ActionResult Index()
        /// </summary>
        /// <returns></returns>
        [Admin]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region 5.0 修改使用者密碼 + ActionResult ChangePassword()
        /// <summary>
        /// 5.0 修改使用者密碼 + ActionResult ChangePassword()
        /// </summary>
        /// <returns></returns>
        [Admin]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        #endregion
        #region 6.0 影片審核頁面 + ActionResult VideoValid()
        /// <summary>
        /// 6.0 影片審核頁面 + ActionResult VideoValid()
        /// </summary>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public ActionResult VideoValid(Entity.PageModel pModel)
        {
            var data = _pli_formDataService.GetList(s => true);
            var totalCount = data.Count();
            var pageIndex = Commons.Helper.CheckIdIsInt(Request.Params["current"].ToString());
            var pageSize = Commons.Helper.CheckIdIsInt(Request.Params["rowCount"].ToString());
            var searchPhrase = Request.Params["searchPhrase"].ToString();
            var sort = Request.Form["sort[pli_videoUid]"] == null ? string.Empty : Request.Form["sort[pli_videoUid]"].ToString();
            if(!string.IsNullOrEmpty(sort) && sort.Equals("desc")) data = data.OrderByDescending(s => s.pli_videoUid).ToList();
            int rid = -1;
            //是True表示為用次序搜尋
            if (int.TryParse(searchPhrase, out rid))
            {
                rid = int.Parse(searchPhrase);
                data = data.Where(s => s.pli_videoUid == rid).ToList();
            }
            //Flase表示用字串查詢
            else if(!string.IsNullOrEmpty(searchPhrase))
            {
                var isCheck = 0;
                switch (searchPhrase)
                {
                    case "通過":
                        isCheck = 1;
                        break;
                    case "未通過":
                        isCheck = 2;
                        break;
                }
                data = data.Where(s => s.pli_videoIsCheck == isCheck).ToList();
            }
            if (pageSize > 0) data = data.Skip((pageIndex -1) * pageSize).Take(pageSize).ToList();
            //if(pModel.Sort ==)
            return WriteJsonToBootGrid(data,pageIndex, pageSize, totalCount);
        }
        #endregion
        #region 7.0 影片審核修改頁面 +ActionResult VideoInfo(string id)
        /// <summary>
        /// 7.0 影片審核修改頁面
        /// </summary>
        /// <param name="id">次序Id</param>
        /// <returns>View(formModel)</returns>
        [Admin]
        [HttpGet]
        public ActionResult VideoInfo(string id)
        {
            var rid = Commons.Helper.CheckIdIsInt(id);
            if (rid == 0 || rid == -1) return WriteJsonErr("請正常進入連結!");
            var formModel = _pli_formDataService.GetList(s => s.pli_videoUid == rid).FirstOrDefault();
            return View(formModel);
        }
        #endregion
        #region 7.1 審核頁面Post + ActionResult VideoInfo(Entity.pli_formData fModel)
        /// <summary>
        /// 7.1 審核頁面Post
        /// </summary>
        /// <param name="fModel">要修改的資料</param>
        /// <returns>Json()</returns>
        [Admin]
        [HttpPost]
        public ActionResult VideoInfo(Entity.pli_formData fModel)
        {
            var model = new Entity.pli_formData()
            {
                pli_videoUid = fModel.pli_videoUid,
                pli_videoIsCheck = fModel.pli_videoIsCheck,
                pli_videoNotPassContent = fModel.pli_videoNotPassContent,
                pli_videoYoutubelink = fModel.pli_videoYoutubelink
            };
            _pli_formDataService.Edit(model, "pli_videoIsCheck", "pli_videoNotPassContent", "pli_videoYoutubelink");
            var result = _pli_formDataService.SaveChanges();
            return result.ContainsKey(1) ? WriteJsonOk("修改成功") : WriteJsonErr("修改失敗,請聯繫系統管理員!");
        } 
        #endregion
        #region 8.0 發信審核通知信
        /// <summary>
        /// 8.0 發信審核通知信
        /// </summary>
        /// <param name="content">審核未通過內容</param>
        /// <param name="sendType">送信類別 1:審核通過信 2:審核未通過信</param>
        /// <param name="uid">次序</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public ActionResult SendMail(string content, string sendType, string uid)
        {
            var rsendType = Commons.Helper.CheckIdIsInt(sendType);
            if (rsendType == 0 || rsendType == -1) return WriteJsonErr("請正常進入連結!");
            var ruid = Commons.Helper.CheckIdIsInt(uid);
            if (ruid == 0 || rsendType == -1) return WriteJsonErr("請正常進入連結!");
            if (string.IsNullOrEmpty(content) && rsendType == 2) return WriteJsonErr("審核不通過需填寫未通過原因!");
            var fModel = _pli_formDataService.GetList(s => s.pli_videoUid == ruid).FirstOrDefault();
            var result = Commons.GmailProvider.SendMail(content, rsendType, fModel.pli_videoUName, fModel.pli_videoEmail,
                fModel.pli_videoFileCustomName);
            var mailModel = new Entity.pli_mailLog
            {
                pil_mailLogReceiveName = fModel.pli_videoUName,
                pil_mailLogReceiveMail = fModel.pli_videoEmail,
                pil_mailLogSendContent = content,
                pli_mailLogFileName = fModel.pli_videoFileCustomName,
                pli_mailLogSendStatu = result ? "送信成功" : "送信失敗,請聯繫系統管理員!",
                pl_mailLogSendType = rsendType == 1,
                pli_mailLogCreateTime = DateTime.Now
            };
            _pli_MailLogService.Add(mailModel);
            fModel.pli_videoMailAlert = true;
            _pli_formDataService.Edit(fModel, "pli_videoMailAlert");
            _pli_MailLogService.SaveChanges();
            return result ? WriteJsonOk("送信成功") : WriteJsonErr("送信失敗,請聯繫系統管理員!"); 
        }
        #endregion
    }
}
