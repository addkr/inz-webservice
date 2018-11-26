using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models;

namespace WebService.Controllers
{
    public class GetController : ApiController
    {
        private healthCenterDBEntities1 db = new healthCenterDBEntities1();

        // GET: api/admins
        [Route("api/admins")]
        [AllowAnonymous]
        public IQueryable<admin> Getadmin()
        {
            return db.admin;
        }

        // GET: api/appointments
        [Route("api/appointments")]
        [AllowAnonymous]
        public IQueryable<appointment> Getappointment()
        {
            return db.appointment;
        }

        // GET: api/doctors
        [Route("api/doctors")]
        [AllowAnonymous]
        public IQueryable<doctor> Getdoctor()
        {
            return db.doctor;
        }



        // GET: api/nurses
        [Route("api/nurses")]
        [AllowAnonymous]
        public IQueryable<nurse> Getnurse()
        {
            return db.nurse;
        }

        // GET: api/patients
        [Route("api/patients")]
        [AllowAnonymous]
        public IQueryable<patient> Getpatient()
        {
            return db.patient;
        }


        // GET: api/receptions
        [Route("api/receptions")]
        [AllowAnonymous]
        public IQueryable<reception> Getreception()
        {
            return db.reception;
        }

        // GET: api/specialities
        [Route("api/specialities")]
        [AllowAnonymous]
        public IQueryable<specialities> Getspecialities()
        {
            return db.specialities;
        }

        [Route("api/CheckUserData")]
        [HttpPost]
        public String CheckUserData(CheckUserNameByAccess checkUserNameByAccess)
        {
            try
            {
                string commandText = "";
                string cs = ConfigurationManager.ConnectionStrings["healthCenterDBConnection"].ConnectionString;
                SqlConnection conn = new SqlConnection(cs);
                conn.Open();
                string result = "";
                switch (checkUserNameByAccess.accesstype)
                {
                    case "doctor":
                        commandText = "Select * from [dbo].[doctor] where [username]=@name";
                        break;
                    case "patient":
                        commandText = "Select * from [dbo].[patient] where [username]=@name";
                        break;
                    case "nurse":
                        commandText = "Select * from [dbo].[nurse] where [username]=@name";
                        break;
                    case "reception":
                        commandText = "Select * from [dbo].[reception] where [username]=@name";
                        break;
                    case "admin":
                        commandText = "Select * from [dbo].[admin] where [username]=@name";
                        break;
                }
               
                SqlCommand command = new SqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserAccessData userAccessData = new UserAccessData();                  
                        userAccessData.lastname = reader.GetString(reader.GetOrdinal("lastname"));
                        userAccessData.forename = reader.GetString(reader.GetOrdinal("forename"));
                        userAccessData.email = reader.GetString(reader.GetOrdinal("email"));
                        userAccessData.username = reader.GetString(reader.GetOrdinal("username"));
                        userAccessData.accesstype = reader.GetString(reader.GetOrdinal("accesstype"));
                        conn.Close();
                        result = JsonConvert.SerializeObject(userAccessData);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
