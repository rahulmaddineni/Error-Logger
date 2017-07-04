using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFinalProject.Models
{
    public class AdminControlModel
    {
    }
    abstract class ApplicationControl
    {
        public abstract bool addUser(UserModel u);
        public abstract bool createnewApp(ApplicationInfo a);
        public abstract bool disableUser(UserModel u, ApplicationModel a);
        public abstract bool assignUser(UserModel u, ApplicationModel a);
    }
}