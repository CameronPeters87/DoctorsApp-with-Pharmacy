using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sprint33.ApiModels
{
    public class AppointmentModel
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentTime { get; set; }
        public bool Confirmed { get; set; }
        public bool CheckedIn { get; set; }
        public string symtoms { get; set; }
        public string diagnosis { get; set; }
        public DateTime Expire { get; set; }
        public bool isPaid { get; set; }
        public bool Complete { get; set; }
    }
}