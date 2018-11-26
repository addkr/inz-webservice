using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    //INPUT
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LoggedOn { get; set; }
    }

    public class CheckUserNameByAccess
    {
        public string username { get; set; }
        public string accesstype { get; set; }
    }

    //OUTPUT
    public class UserAccessData
    {
        public string forename { get; set; }
        public string lastname { get; set; }
        public string accesstype { get; set; }
        public string email { get; set; }
        public string username { get; set; }
    }

}