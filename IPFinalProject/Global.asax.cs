using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CommonCode;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace IPFinalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public object ExceptionUtility { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        /// <summary>
        /// Code that runs when an unhandled error occurs
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            Logger logger = new Logger();

            String ipAddress = HttpContext.Current.Request.UserHostAddress;
            String useremail = User.Identity.Name;

            // Handle HTTP errors
            if (exc.GetType() == typeof(HttpException))
            {
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;

                logger.LogSystemError(useremail, ipAddress, exc);

                //Redirect HTTP errors to HttpError page
                String BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);
                Response.Redirect("~/Home/page404");
            }
            else
            {
                logger.LogSystemError(useremail, ipAddress, exc);

                String BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);
                Response.Redirect("~/Home/page500");
            }

            // Clear the error from the server
            Server.ClearError();
        }
    }
}
