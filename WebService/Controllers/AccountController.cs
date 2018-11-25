using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebService.Models;
using System.Configuration;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace WebService.Controllers
{
    public class AccountController : ApiController
    {
        [Route("api/User/Register")]
        [HttpPost]
        [AllowAnonymous]
        public IdentityResult Register(UserModel model)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email };
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            IdentityResult result = manager.Create(user, model.Password);
            return result;
        }

        [HttpGet]
        [Route("api/GetUserClaims")]
        public UserModel GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            UserModel model = new UserModel()
            {
                UserName = identityClaims.FindFirst("Username").Value,
                Email = identityClaims.FindFirst("Email").Value,
                LoggedOn = identityClaims.FindFirst("LoggedOn").Value
            };
            return model;
        }

        [Route("api/CheckUserData")]
        [HttpPost]        
        [AllowAnonymous]
        public String CheckUserData(UserName username)
        {
            try
            {
                
                string cs = ConfigurationManager.ConnectionStrings["healthCenterDBConnection"].ConnectionString;
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();
                string result = "";
                SqlCommand command = new SqlCommand("Select * from [dbo].[patient] where [forename]=@name", conn);
                command.Parameters.AddWithValue("@name", username.userName);
                patient patient = new patient();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserAccessData userAccessData = new UserAccessData();
                        userAccessData.lastname = reader.GetString(2);
                        userAccessData.forename = reader.GetString(0);
                        userAccessData.email = reader.GetString(10);
                        //userAccessData.userName = reader.GetString(18);
                        //userAccessData.accessType = reader.GetString(19);
                        conn.Close();
                        result =  JsonConvert.SerializeObject(userAccessData);
                        
                    }

                }
                return result;

            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }


    }

   
}
