using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sprint33.Models.ViewModel
{
    public class AppointmentVM
    {
        public int AppointmentID { get; set; }

        [Display(Name = "Patient`s Name")]
        public string PatientName { get; set; }
        [Display(Name = "Doctor`s Name")]
        public string DoctorName { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Day and Time of Appointment")]
        public DateTime AppointmentTime { get; set; }
        public bool Confirmed { get; set; }
        public bool isPaid { get; set; }
        public bool Complete { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }

    }

    public class AdminAppointmentsListVM
    {
        public int AppointmentID { get; set; }

        [Display(Name = "Patient`s Name")]
        public string PatientName { get; set; }
        [Display(Name = "Doctor`s Name")]
        public string DoctorName { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Day and Time of Appointment")]
        public DateTime AppointmentTime { get; set; }
        public bool Confirmed { get; set; }
        public bool isPaid { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }

    }
}