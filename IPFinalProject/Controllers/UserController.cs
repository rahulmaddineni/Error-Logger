using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoadersandLogic;
using ErrorLoggerModel;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;
using PagedList;
using CommonCode;
using System.Net;

namespace IPFinalProject.Controllers
{
    //[Authorize]
    [Authorize(Roles = "User")]

    public class UserController : Controller
    {
        Logger logger = new Logger();
        static List<ErrorLogModel> pagedData = null;

        //Check User Status
        public bool getUserStatus()
        {
            var useremail = User.Identity.GetUserName();
            UserDataHandler dataSource = new UserDataHandler();

            return dataSource.GetUserByEmail(useremail).status;
        }


        //GET: User
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var useremail =  User.Identity.GetUserName();
            ApplicationDataHandler dataSource = new ApplicationDataHandler();
            UserDataHandler dataSource1 = new UserDataHandler();

            var user = dataSource1.GetUserByEmail(useremail);
            if (user.status == false)
            {
                Response.Redirect("/Home/pageUserDisabled");
            }
            
            ICollection<ApplicationModel> data = dataSource.GetAllApplicationsByUserEmail(useremail);
            ViewBag.UserName = user.firstName + ' ' + user.lastName;
            ViewBag.UserId = user.userId;
            ViewBag.UserEmail = user.email;
            ViewBag.UserStatus = user.status;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.appName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(s => s.appName).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.appName).ToList();
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        public ActionResult AddUser(string email)
        {
            UserModel user = new UserModel();
            user.email = email;
            return View(user);
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUser(UserModel newUser)
        {
            newUser.status = true;
            newUser.role = true;
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();
                dataSource.AddUser(newUser);
                
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.App‌​licationCookie);
                
                TempData["UserCreatedNotification"] = "New User created: " + newUser.email + " Login to use the application";
                TempData["ShowNotification"] = "true";

                return RedirectToAction("Index","Home");
            }

            return View(newUser);
        }

        public ActionResult GetErrorLogs(int userId, int appId, string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (getUserStatus() == false)
            {
                Response.Redirect("/Home/pageUserDisabled");
            }

            ErrorDataHandler erdh = new ErrorDataHandler();
            ApplicationDataHandler apdh = new ApplicationDataHandler();
            UserDataHandler dataSource1 = new UserDataHandler();

            var useremail = User.Identity.GetUserName();
            var uId = dataSource1.GetUserByEmail(useremail).userId;
            List<ErrorLogModel> data = new List<ErrorLogModel>();

            if (apdh.CheckUserBelongsToApplication(userId, appId) && uId == userId)
            {
                data = erdh.getLogsById(appId);
                ViewBag.appId = appId;
                ViewBag.AppName = apdh.GetAppById(appId).appName;
                ViewBag.RestrictedAccess = false;
                ViewBag.userId = userId;
            }
            else
            {
                // user caught trying to access restricted app data
                logger.LogSystemError("UnAuthorized Access: " + User.Identity.Name + "Tried to access User Id: " + userId + "'s App Id: " + appId);
                ViewBag.appId = appId;
                ViewBag.userId = userId;
                ViewBag.RestrictedAccess = true;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.LogSortParm = sortOrder == "Log" ? "log_desc" : "Log";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.errorDescription.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(s => s.errorDescription).ToList();
                    break;
                case "Date":
                    data = data.OrderBy(s => s.errorTime).ToList();
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.errorTime).ToList();
                    break;
                case "Log":
                    data = data.OrderBy(s => s.logLevel).ToList();
                    break;
                case "log_desc":
                    data = data.OrderByDescending(s => s.logLevel).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.errorDescription).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            pagedData = data.ToPagedList(pageNumber, pageSize).ToList();
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetChart()
        {
            ErrorDataHandler erdh = new ErrorDataHandler();
            List<String> data = erdh.getErrorDataForChart(pagedData);
            var myChart = new Chart(width: 600, height: 400)
            .AddTitle("Exception Chart")
            .AddLegend(" ")
            .AddSeries(
                name: "Type of Errors",
                chartType: "Pie",
                xValue: new[] { "Errors with Exceptions", "Errors without Exceptions" },
                yValues: new[] { data[1], data[0] })
            .Write("png");
            return null;
        }

        public ActionResult GetBarChart()
        {
            ErrorDataHandler erdh = new ErrorDataHandler();
            List<String> data = erdh.getErrorDataForBarChart(pagedData);
            var myChart = new Chart(width: 600, height: 400, theme:ChartTheme.Vanilla)
           .AddTitle("Log Chart")
           .AddSeries(
               name: "Log Level",
               xValue: new[] { "DEBUG", "INFO", "WARNING"},
               yValues: new[] { data[0], data[1], data[2] })
           .Write("png");
            return null;
        }

        public ActionResult CreateError()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult CreateError(int appId, ErrorLogModel errToSave)
        {
            if (ModelState.IsValid)
            {
                errToSave.ApplicationRefId = appId;

                ErrorDataHandler dataSource1 = new ErrorDataHandler();
                dataSource1.AddLogs(errToSave);

                return RedirectToAction("Index");
            }

            return View(errToSave);
        }

    }
}