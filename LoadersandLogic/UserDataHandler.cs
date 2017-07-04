using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorLoggerModel;
using System.Web.Http;
using System.Net.Http;

namespace LoadersandLogic
{
    public class UserDataHandler
    {
        public UserDataHandler()
        {
            
        }
        
        public UserModel GetUsersByUserId(int userId)
        {
            using (var err = new ErrorModel())
            {
                if (err.Users.Any(x => x.userId == userId))
                    return err.Users.First(x => x.userId == userId);
                else
                    return new UserModel();
            }
        }

        public UserModel GetUserByEmail(string useremail)
        {
            using (var err = new ErrorModel())
            {
                if (err.Users.Any(x => x.email == useremail))
                    return err.Users.First(x => x.email == useremail);
                else
                    return new UserModel();
            }
        }

        public ICollection<UserModel> GetUsersByAppId(int id)
        {
            using (var err = new ErrorModel())
            {
                if (err.Applications.Any(x => x.appId == id))
                    return err.Applications.First(x => x.appId == id).Users.ToList();
                else
                    return new List<UserModel>();
            }
        }

        public ICollection<UserModel> GetRemainingUsers(int id)
        {
            using (var err = new ErrorModel())
            {
                ICollection<UserModel> app = err.Applications.First(x => x.appId == id).Users.ToList();
                ICollection<UserModel> users = GetAllUsers();
                return users.Where(p => !app.Any(p2 => p2.userId == p.userId)).Where(y => y.status == true).ToList();
            }
        }

        public ICollection<UserModel> GetAllUsers()
        {
            using (var err = new ErrorModel())
            {
                return err.Users.ToList();
            }
        }

        public bool AddUser(UserModel userToSave)
        {
            UserModel user = userToSave;
            using (var err = new ErrorModel())
            {
                if (err.Users.Any(x => x.email == user.email))
                {
                    err.Users.First(x => x.email == user.email).lastLoginDate = DateTime.Now;
                    err.SaveChanges();
                }
                else
                {
                    user.Applications = new List<ApplicationModel>();
                    err.Users.Add(user);
                    err.SaveChanges();
                }
            }
            return false;
        }

        public bool EnableUser(int id)
        {
            using (var err = new ErrorModel())
            {
                err.Users.First(x => x.userId == id).status = true;
                err.SaveChanges();
            }
            return true;
        }

        public bool DisableUser(int id)
        {
            using (var err = new ErrorModel())
            {
                err.Users.First(x => x.userId == id).status = false;
                err.SaveChanges();
            }
            return true;
        }

        public bool DeleteUserById(int id)
        {
            using (var err = new ErrorModel())
            {
                if (err.Users.Any(x => x.userId == id))
                {
                    UserModel userToRemove = err.Users.First(x => x.userId == id);
                    err.Users.Remove(userToRemove);
                    err.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public void setUserLastLogin(string useremail)
        {
            using (var err = new ErrorModel())
            {
                if (err.Users.Any(x => x.email == useremail))
                {
                    err.Users.First(x => x.email == useremail).lastLoginDate = DateTime.Now;
                    err.SaveChanges();
                }
            }
        }

    }
}