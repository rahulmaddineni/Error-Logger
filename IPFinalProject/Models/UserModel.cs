using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFinalProject.Models
{
    public class UserModel
    {
    }
    public class BasicInfo
    {
        public string firstname;
        public string lastname;
        public string password;
        public string email;
        BasicInfo()
        {
            this.firstname = "Rahul";
            this.lastname = "Maddineni";
            this.password = "*********";
            this.email = "maddineni.rahulbabu@gmail.com";
        }
        public string getFirstName()
        {
            return this.firstname;
        }
        public string getLastName()
        {
            return this.lastname;
        }
        public void setLastName(string last)
        {
            this.lastname = last;
        }
        public void setFirstName(string first)
        {
            this.firstname = first;
        }

    }
    
    abstract class ViewErrors
    {
        public abstract bool viewErrors();
        public abstract bool viewGraphs();
    }
}