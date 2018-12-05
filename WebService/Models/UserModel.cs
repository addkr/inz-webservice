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

    public class CheckDoctorBySpeciality
    {
        public string speciality { get; set; }
    }

    //OUTPUT
    public class UserAccessData
    {
        public string forename { get; set; }
        public string lastname { get; set; }
        public string accesstype { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string pesel { get; set; }
    }

    public class FreeTerms
    {
        public string date { get; set; }
        public string doctorforename { get; set; }
        public string doctorlastname { get; set; }
        public string doctorspeciality { get; set; }
        public string id { get; set; }
    }

    public class Doctor
    {
        public string doctorforename { get; set; }
        public string doctorlastname { get; set; }
        public string doctorspeciality { get; set; }
        public string doctorusername { get; set; }
    }

   
    public class Appointment
    {
        public bool edit { get; set; }
        public string date { get; set; }
        public string confirmed { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string patientusername { get; set; }
        public string doctorusername { get; set; }
    }

}