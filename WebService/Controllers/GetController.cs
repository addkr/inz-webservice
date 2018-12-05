using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models;

namespace WebService.Controllers
{
    public class GetController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["healthCenterDBConnection"].ConnectionString;

        private healthCenterDBEntities db = new healthCenterDBEntities();

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
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                string result = "";
                commandText = "Select * from [dbo].["+ checkUserNameByAccess.accesstype+"] where [username]=@name";
                SqlCommand command = new SqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserAccessData userAccessData = new UserAccessData();
                        if (checkUserNameByAccess.accesstype == "admin")
                        {
                            userAccessData.email = reader.GetString(reader.GetOrdinal("email"));
                            userAccessData.username = reader.GetString(reader.GetOrdinal("username"));
                            userAccessData.accesstype = reader.GetString(reader.GetOrdinal("accesstype"));
                            conn.Close();
                            result = JsonConvert.SerializeObject(userAccessData);
                        }
                        else
                        {
                            userAccessData.lastname = reader.GetString(reader.GetOrdinal("lastname"));
                            userAccessData.forename = reader.GetString(reader.GetOrdinal("forename"));
                            userAccessData.email = reader.GetString(reader.GetOrdinal("email"));
                            userAccessData.username = reader.GetString(reader.GetOrdinal("username"));
                            userAccessData.accesstype = reader.GetString(reader.GetOrdinal("accesstype"));
                            conn.Close();
                            result = JsonConvert.SerializeObject(userAccessData);
                        }                                        
                        
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [Route("api/GetPatients")]
        [HttpPost]
        public ArrayList GetPatients(UserAccessData checkUserNameByAccess)
        {
            try
            {
                string commandText = "";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                if(checkUserNameByAccess.accesstype == "doctor")
                {
                    commandText = "Select * from [dbo].[patient] where [doctorusername]=@name";
                }
                else if(checkUserNameByAccess.accesstype == "nurse")
                {
                    commandText = "Select * from [dbo].[patient] where [nurseusername]=@name";
                }
                else if (checkUserNameByAccess.accesstype == "reception")
                {
                    if(checkUserNameByAccess.username != null)
                    {
                        commandText = "Select * from [dbo].[patient] where [username]=@name";
                    }
                    else if(checkUserNameByAccess.pesel != null)
                    {
                        commandText = "Select * from [dbo].[patient] where [pesel]=@pesel";
                    }
                    
                }

                SqlCommand command = new SqlCommand(commandText, conn);
                if(checkUserNameByAccess.username != null){
                    command.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                }
                if(checkUserNameByAccess.pesel != null)
                {
                    command.Parameters.AddWithValue("@pesel", checkUserNameByAccess.pesel);
                }
                
                ArrayList arrayList = new ArrayList();
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                
                foreach (DataRow row in dt.Rows)
                {
                    UserAccessData userAccessData = new UserAccessData();
                    userAccessData.lastname = row.ItemArray[2].ToString();
                    userAccessData.forename = row.ItemArray[0].ToString();
                    userAccessData.email = row.ItemArray[10].ToString();
                    userAccessData.pesel = row.ItemArray[8].ToString();
                    userAccessData.username = row.ItemArray[18].ToString();
                    arrayList.Add(userAccessData);
                }

                return arrayList;
            }
            catch (Exception ex)
            {
                ArrayList array = new ArrayList();
                array.Add(ex.Message.ToString());
                return array;
            }
        }

        [Route("api/GetFreeTerms")]
        [HttpPost]
        public ArrayList GetFreeTerms(CheckUserNameByAccess checkUserNameByAccess)
        {
            try
            {
                string commandText = "Select * from [dbo].[freeterms] where [doctorusername]=@name";
                string commandText1 = "Select * from [dbo].["+checkUserNameByAccess.accesstype+"] where [username]=@name";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(commandText, conn);
                SqlCommand command1 = new SqlCommand(commandText1, conn);
                command.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                command1.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                ArrayList arrayList = new ArrayList();
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                string forename = "", lastname = "";

                using (SqlDataReader reader = command1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lastname = reader.GetString(reader.GetOrdinal("lastname"));
                        forename = reader.GetString(reader.GetOrdinal("forename"));
                    }
                }
                        foreach (DataRow row in dt.Rows)
                {
                    FreeTerms freeTerm = new FreeTerms();
                    freeTerm.doctorforename = forename;
                    freeTerm.doctorlastname = lastname;
                    freeTerm.id = row.ItemArray[0].ToString();
                    freeTerm.date = row.ItemArray[2].ToString();
                    freeTerm.doctorspeciality = row.ItemArray[3].ToString();
                    arrayList.Add(freeTerm);
                }

                return arrayList;
            }
            catch (Exception ex)
            {
                ArrayList array = new ArrayList();
                array.Add(ex.Message.ToString());
                return array;
            }
        }

        [Route("api/GetDoctorsBySpeciality")]
        [HttpPost]
        public ArrayList GetDoctorsBySpeciality(CheckDoctorBySpeciality checkDoctorBySpeciality)
        {
            try
            {
                string commandText = "";
                string cs = ConfigurationManager.ConnectionStrings["healthCenterDBConnection"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                if(checkDoctorBySpeciality.speciality == "Pielęgniarka")
                {
                    commandText = "Select * from [dbo].[nurse] where [speciality]=@name";
                }
                else
                {
                    commandText = "Select * from [dbo].[doctor] where [speciality]=@name";
                }
                
                SqlCommand command = new SqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@name", checkDoctorBySpeciality.speciality);
                ArrayList arrayList = new ArrayList();
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());

                foreach (DataRow row in dt.Rows)
                {
                    Doctor doctor = new Doctor();
                    doctor.doctorforename = row.ItemArray[0].ToString();
                    doctor.doctorlastname = row.ItemArray[1].ToString();
                    doctor.doctorspeciality = row.ItemArray[2].ToString();
                    doctor.doctorusername = row.ItemArray[3].ToString();
                    arrayList.Add(doctor);
                }

                return arrayList;
            }
            catch (Exception ex)
            {
                ArrayList array = new ArrayList();
                array.Add(ex.Message.ToString());
                return array;
            }
        }

        [Route("api/DeleteFreeTerm")]
        [HttpPost]
        public string DeleteFreeTerm(FreeTerms freeTerms)
        {
            try
            {
                
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM [dbo].[freeterms] WHERE [id] = '" + freeTerms.id + "'", con))
                    {
                        command.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return "OK";
            }
            catch (SystemException ex)
            {
                return ex.ToString();
            }
        }


        [Route("api/GetPatientAppointments")]
        [HttpPost]
        public ArrayList GetPatientAppointments(CheckUserNameByAccess checkUserNameByAccess)
        {
            try
            {
                string commandText = "";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
               

                commandText = "Select * from [dbo].[patient] where [patientusername]=@name";
                SqlCommand command = new SqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                ArrayList arrayList = new ArrayList();
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());

                foreach (DataRow row in dt.Rows)
                {
                    UserAccessData userAccessData = new UserAccessData();
                    userAccessData.lastname = row.ItemArray[0].ToString();
                    userAccessData.forename = row.ItemArray[4].ToString();
                    userAccessData.email = row.ItemArray[10].ToString();
                    arrayList.Add(userAccessData);
                }

                return arrayList;
            }
            catch (Exception ex)
            {
                ArrayList array = new ArrayList();
                array.Add(ex.Message.ToString());
                return array;
            }
        }

        [Route("api/GetAppointmentsList")]
        [HttpPost]
        //[AllowAnonymous]
        public ArrayList GetAppointmentsList(CheckUserNameByAccess checkUserNameByAccess)
        {
            try
            {
                string commandText = "";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                if(checkUserNameByAccess.accesstype == "doctor")
                {
                    commandText = "Select * from [dbo].[appointment] where [medicusername]=@name";
                }
                else if (checkUserNameByAccess.accesstype == "patient")
                {
                    commandText = "Select * from [dbo].[appointment] where [patientusername]=@name";
                }
                
                SqlCommand command = new SqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@name", checkUserNameByAccess.username);
                ArrayList arrayList = new ArrayList();
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());

                foreach (DataRow row in dt.Rows)
                {
                    
                    Appointment appointment = new Appointment();
                    appointment.date =  row.ItemArray[0].ToString();
                    //CultureInfo culture = new CultureInfo("de-DE");
                    DateTime tempDate;
                    CultureInfo fromCulture = new CultureInfo("en-US");
                    DateTime dateTime = DateTime.Parse(appointment.date);
                    appointment.date = dateTime.ToString(CultureInfo.GetCultureInfo("en-US"));
                    appointment.doctorusername = row.ItemArray[4].ToString();
                    appointment.patientusername = row.ItemArray[3].ToString();
                    appointment.id = row.ItemArray[1].ToString();
                    appointment.description = row.ItemArray[2].ToString();
                    appointment.confirmed = row.ItemArray[6].ToString();
                    arrayList.Add(appointment);
                }

                return arrayList;
            }
            catch (Exception ex)
            {
                ArrayList array = new ArrayList();
                array.Add(ex.Message.ToString());
                return array;
            }
        }

        //        command.CommandText = "UPDATE Student 
        //SET Address = @add, City = @cit Where FirstName = @fn and LastName = @add";

        [Route("api/UpdateAppointment")]
        [HttpPost]
        public String UpdateAppointment(Appointment updateParams)
        {
            try
            {
                string commandText = "";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                if (updateParams.edit == true)
                {
                    commandText = "UPDATE [dbo].[appointment] SET [date]=@date, [description]=@description where [id]=@id";
                }
                else 
                {
                    commandText = "UPDATE [dbo].[appointment] SET [confirmed]=@confirmed, [description]=@description where [id]=@id";
                }

                SqlCommand command = new SqlCommand(commandText, conn);
                command.Parameters.AddWithValue("@date", updateParams.date);
                command.Parameters.AddWithValue("@id", updateParams.id);
                command.Parameters.AddWithValue("@description", updateParams.description);
                command.Parameters.AddWithValue("@confirmed", updateParams.confirmed);               

                command.ExecuteNonQuery();
                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
