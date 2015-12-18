using IdentitySample.Models;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdentitySample
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES");
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            ErrorConfig.Handle(Context);
        }
        public class ErrorConfig
        {
            public static void Handle(HttpContext context)
            {
                switch (context.Response.StatusCode)
                {
                    //Not authorized
                    case 401:
                        Show(context, 401);
                        break;

                    //Not found
                    case 404:
                        Show(context, 404);
                        break;
                }
            }

            static void Show(HttpContext context, Int32 code)
            {
                context.Response.Clear();

                var w = new HttpContextWrapper(context);
                var c = new ErrorController() as IController;
                var rd = new RouteData();

                rd.Values["controller"] = "Error";
                rd.Values["action"] = "Index";
                rd.Values["id"] = code.ToString();

                c.Execute(new RequestContext(w, rd));
            }
        }
        internal class ErrorController : Controller
        {
            [HttpGet]
            public ViewResult Index(Int32? id)
            {
                var statusCode = id.HasValue ? id.Value : 500;
                var error = new HandleErrorInfo(new Exception("Una exepcion a ocurrido con el error" + statusCode + "!"), "Error", "Index");
                return View("Error", error);
            }
        }
    }
}
