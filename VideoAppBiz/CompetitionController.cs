namespace VideoApp.Biz
{
    using System;
    using System.Linq;
    using IServices;
    using System.Web.Mvc;

    public class CompetitionController : JsonController
    {
        readonly string _SavePath = System.Configuration.ConfigurationManager.AppSettings["FileFolder"];
        readonly string _videoUrl = System.Configuration.ConfigurationManager.AppSettings["VideoUrl"];
        private Ipli_formDataService _pliFormDataService;

        public CompetitionController(Ipli_formDataService pliFormDataService)
        {
            this._pliFormDataService = pliFormDataService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VideoUpload()
        {
            //文件保存目錄路徑 
            var files = Request.Files;
            var fileSize = Convert.ToInt32(Request.Form["size"]) / 1000000;
            if (files.Count <= 0) return WriteJsonErr("請選擇附件!");
            if (fileSize >= 1500) return WriteJsonErr("影片過大,請重新壓縮再上傳!!");
            var tempFileName = files[0].FileName.Split('\\');
            //檔案名稱
            var oriName = tempFileName[tempFileName.Length - 1];
            //驗證檔案附檔名
            if (!oriName.ToLower().Contains("mp4")) return WriteJsonErr("僅能上傳MP4格式檔案!");
            //去除特殊字元
            var fileArr = tempFileName[tempFileName.Length - 1].Split(new[] { " ", "&", "=", "!", "$", "'", "~", "%", "+", "`", "^", "(", "[", "{", "}", "]", ")", ";", ",", "#", "!", "`" }, System.StringSplitOptions.RemoveEmptyEntries);
            // 3^6=2c9+4&8(e5]5{c = e'3;4,3~+da%0$3e#8f@f!b`258ecfaa
            var filename = fileArr.Aggregate("", (current, s) => current + (s + "_"));
            filename = filename.TrimEnd('_');
            var newName = DateTime.Now.ToString("yyyyMMddHHmm") + Guid.NewGuid().ToString().Substring(0, 4) + "_" + filename;
            var pathForSaving = _SavePath + newName;
            files[0].SaveAs(pathForSaving);
            Session.Add("formData", new Entity.pli_formData()
            {
                pli_videoFileOriName = oriName,
                pli_videoFileNewName = newName,
                pli_videoFileUploadTime = DateTime.Now,
                pli_videoFileSize = fileSize + "M"

            });
            return WriteJsonOk("上傳成功!");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FormSubmit(Entity.pli_formData formModel)
        {
            if (Session["formData"] == null) return WriteJsonErr("請先上傳影片,並耐心等候影片上傳完成!");
            var model = Session["formData"] as Entity.pli_formData;
            formModel.pli_videoFileOriName = model.pli_videoFileOriName;
            formModel.pli_videoFileNewName = model.pli_videoFileNewName;
            formModel.pli_videoFileSize = model.pli_videoFileSize;
            formModel.pli_videoFileUploadTime = model.pli_videoFileUploadTime;
            formModel.pli_videoFileUrl = _videoUrl + model.pli_videoFileNewName;
            formModel.pli_videoGameId = "1";//先給假的
            _pliFormDataService.Add(formModel);
            var result = _pliFormDataService.SaveChanges();
            return result.ContainsKey(-1) ? WriteJsonErr("送出錯誤,請稍候再試"):WriteJsonOk("報名成功!");
        }
    }
}
