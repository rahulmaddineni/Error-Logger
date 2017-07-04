using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;
using System.Web;

namespace LoadersandLogic
{
    public class ApplicationDataHandler
    {
        public ApplicationDataHandler()
        {
     
        }

        public ApplicationModel GetApplicationByAppName(String appName)
        {
            using (var err = new ErrorModel())
            {
                return err.Applications.First(x => x.appName == appName);
            }
        }

        public ApplicationModel GetAppById(int id)
        {
            using (var err = new ErrorModel())
            {
                return err.Applications.First(x => x.appId == id);
            }
        }

        // All Applications in Database
        public ICollection<ApplicationModel> GetAllApplications()
        {
            using (var err = new ErrorModel())
            {
                return err.Applications.ToList();
            }
        }

        public ICollection<ApplicationModel> GetAllApplicationsByUserEmail(String userEmail)
        {
            using (var err = new ErrorModel())
            {
                return err.Applications.Where(x => x.status).Where(x => x.Users.Any(y => y.email == userEmail)).ToList();
            }
        }

        public bool AddApp(ApplicationModel appToSave)
        {
            using (var err = new ErrorModel())
            {
                if (!err.Applications.Any(x => x.appName == appToSave.appName))
                {
                    appToSave.Users = new List<UserModel>();
                    appToSave.Errors = new List<ErrorLogModel>();
                    err.Applications.Add(appToSave);
                    err.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool AddUserToApp(int appId, int userId)
        {
            using (var err = new ErrorModel())
            {
                if (err.Applications.Any(x => x.appId == appId))
                {
                    if (!err.Applications.First(x => x.appId == appId).Users.Any(x => x.userId == userId))
                    {
                        err.Applications.First(x => x.appId == appId).Users.Add(err.Users.First(y => y.userId == userId));
                        err.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckUserBelongsToApplication(int userId, int appId)
        {
            
            using (var err = new ErrorModel())
            {
                if (err.Applications.Any(x => x.appId == appId))
                {
                    if (err.Applications.First(x => x.appId == appId).Users.Any(y => y.userId == userId))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool EnableApp(int id)
        {
            using (var err = new ErrorModel())
            {
                err.Applications.First(x => x.appId == id).status = true;
                err.SaveChanges();
            }
            return true;
        }

        public bool DisableApp(int id)
        {
            using (var err = new ErrorModel())
            {
                err.Applications.First(x => x.appId == id).status = false;
                err.SaveChanges();
            }
            return true;
        }

        public bool DeleteAppById(int id)
        {
            using (var err = new ErrorModel())
            {
                if(err.Applications.Any(x => x.appId == id))
                {
                    ApplicationModel appToRemove = err.Applications.First(x => x.appId == id);
                    err.Applications.Remove(appToRemove);
                    err.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        
    }
}

