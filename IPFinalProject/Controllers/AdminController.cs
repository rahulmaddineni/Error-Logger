using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErrorLoggerModel;
using LoadersandLogic;
using PagedList;
using System.Web.Helpers;

namespace IPFinalProject.Controllers
{
    //[Authorize]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        static List<ErrorLogModel> pagedData = null;

        // GET: Admin
        public ActionResult Index()
        {
            ApplicationDataHandler dataSource1 = new ApplicationDataHandler();            
            UserDataHandler dataSource2 = new UserDataHandler();
            
            List<String> data = new List<String>();
            data.Add(dataSource1.GetAllApplications().Count.ToString());
            data.Add(dataSource2.GetAllUsers().Count.ToString());

            return View(data);
        }

        public ActionResult ViewApplications(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if(TempData["ShowNotification"] as String == "true")
            {
                ViewBag.AppNotification = TempData["AppCreatedNotification"] as String;
            }

            ApplicationDataHandler dataSource = new ApplicationDataHandler();
            ICollection<ApplicationModel> data = dataSource.GetAllApplications();
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.appName.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(s => s.appName).ToList();
                    break;
                case "Date":
                    data = data.OrderBy(s => s.createdDate).ToList();
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.createdDate).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.appName).ToList();
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ViewApplicationDetails(int id)
        {
            ApplicationDataHandler dataSource = new ApplicationDataHandler();
            UserDataHandler dataSource2 = new UserDataHandler();

            var data = new AppAndUserModel { app = dataSource.GetAppById(id), users = dataSource2.GetUsersByAppId(id) };

            return View(data);
        }


        public ActionResult CreateApplication()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult CreateApplication(ApplicationModel newApp)
        {
            newApp.status = true;
            if (ModelState.IsValid)
            {
                ApplicationDataHandler dataSource = new ApplicationDataHandler();
                if (dataSource.AddApp(newApp))
                {
                    TempData["AppCreatedNotification"] = "New App created: " + newApp.appName;
                }
                else
                {
                    TempData["AppCreatedNotification"] = "App can not be created";
                }
                TempData["ShowNotification"] = "true";
                return RedirectToAction("ViewApplications");
            }

            return View(newApp);
        }

        public ActionResult DeleteApplication()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteApplication(int id)
        {
            if (ModelState.IsValid)
            {
                ApplicationDataHandler dataSource = new ApplicationDataHandler();
                dataSource.DeleteAppById(id);

                return RedirectToAction("ViewApplications");
            }

            return View(id);
        }

        public ActionResult EnableApplication()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EnableApplication(int id)
        {
            if (ModelState.IsValid)
            {
                ApplicationDataHandler dataSource = new ApplicationDataHandler();
                dataSource.EnableApp(id);

                return RedirectToAction("ViewApplications");
            }

            return View(id);
        }

        public ActionResult DisableApplication()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DisableApplication(int id)
        {
            if (ModelState.IsValid)
            {
                ApplicationDataHandler dataSource = new ApplicationDataHandler();
                dataSource.DisableApp(id);

                return RedirectToAction("ViewApplications");
            }

            return View(id);
        }

        public ActionResult GetErrorLogs(int id, string sortOrder, string searchString, string currentFilter, int? page)
        {
            ErrorDataHandler erdh = new ErrorDataHandler();
            ApplicationDataHandler apdh = new ApplicationDataHandler();

            List<ErrorLogModel> data = erdh.getLogsById(id);
            ViewBag.appId = id;
            ViewBag.AppName = apdh.GetAppById(id).appName;

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

        public ActionResult ViewUsers(string sortOrder, string searchString, string currentFilter, int? page)
        {
            UserDataHandler dataSource = new UserDataHandler();
            ICollection<UserModel> data = dataSource.GetAllUsers();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.firstName.Contains(searchString) || s.lastName.Contains(searchString) || s.phone.Contains(searchString) || s.email.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(s => s.firstName).ToList();
                    break;
                case "Date":
                    data = data.OrderBy(s => s.lastLoginDate).ToList();
                    break;
                case "date_desc":
                    data = data.OrderByDescending(s => s.lastLoginDate).ToList();
                    break;
                default:
                    data = data.OrderBy(s => s.firstName).ToList();
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(data.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult EnableUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EnableUser(int id)
        {
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();
                dataSource.EnableUser(id);

                return RedirectToAction("ViewUsers");
            }

            return View(id);
        }

        public ActionResult DisableUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DisableUser(int id)
        {
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();
                dataSource.DisableUser(id);

                return RedirectToAction("ViewUsers");
            }

            return View(id);
        }

        public ActionResult DeleteUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteUser(int id)
        {
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();
                dataSource.DeleteUserById(id);

                return RedirectToAction("ViewUsers");
            }

            return View(id);
        }

        public ActionResult AssignUsers(int id)
        {
            UserDataHandler dataSource = new UserDataHandler();
            ICollection<UserModel> data = dataSource.GetRemainingUsers(id);
            ViewBag.appId = id;

            return View(data);
        }

        public ActionResult AssignUserToApp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AssignUserToApp(int appId, int userId)
        {
            if (ModelState.IsValid)
            {
                ApplicationDataHandler dataSource1 = new ApplicationDataHandler();

                dataSource1.AddUserToApp(appId , userId);
                return RedirectToAction("ViewApplications");
            }

            return AssignUsers(appId);
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
            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
           .AddTitle("Log Chart")
           .AddSeries(
               name: "Log Level",
               xValue: new[] { "DEBUG", "INFO", "WARNING" },
               yValues: new[] { data[0], data[1], data[2] })
           .Write("png");
            return null;
        }

    }
}