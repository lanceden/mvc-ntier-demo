namespace VideoApp.Biz
{
    using System.Web.Mvc;
    public class BaseController:Controller
    {
        readonly string loginUrl = System.Configuration.ConfigurationManager.AppSettings["AdminLoginUrl"];
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.ActionDescriptor.IsDefined(typeof(AdminAttribute),false))
            {
                if (Session["Admin"] == null)
                {
                    var content = new ContentResult()
                    {
                        Content = "<script>alert('請先登入!');window.location.href = '" + loginUrl + "';</script>"
                    };
                    filterContext.Result = content;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
