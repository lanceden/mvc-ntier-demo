using System;

namespace VideoApp.Biz
{
    using System.Web.Mvc;
    public class JsonController :BaseController
    {
        /// <summary>
        /// 寫入Json訊息
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        /// <returns></returns>
        public JsonResult WriteJsonOk(string msg)
        {
            return Json(new { statu = "ok", msg = msg }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 寫入Json訊息
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        /// <param name="data">要回傳的資料</param>
        /// <returns></returns>
        public JsonResult WriteJsonOk(string msg, object data)
        {
            return Json(new { statu = "ok", msg = msg, data = data }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 寫入Json訊息
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        /// <returns></returns>
        public JsonResult WriteJsonErr(string msg)
        {
            return Json(new { statu = "err", msg = msg }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 寫入Json訊息
        /// </summary>
        /// <param name="msg">要顯示的訊息</param>
        /// <returns></returns>
        public JsonResult WriteJsonErr(Exception ex)
        {
            var exp = ex.InnerException ?? ex;
            while (exp.InnerException != null)
            {
                exp = ex.InnerException;
            }
            return Json(new { statu = "err", msg = exp.Message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WriteJsonToBootGrid(object data,int pageIndex,int pageSize, int totalCount)
        {
            return Json(new
            {
                current= pageIndex,
                rowCount = pageSize,
                rows = data,
                total = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
