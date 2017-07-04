using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class AppAndUserModel
    {
        public ApplicationModel app;
        public ICollection<UserModel> users;
    }

}
