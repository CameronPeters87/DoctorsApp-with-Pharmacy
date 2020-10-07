using System;
using System.ComponentModel.DataAnnotations;

namespace Sprint33.Models.ViewModel
{
    public class ConsultationVM
    {
    }

    public class ConsultationListVM
    {
        public ConsultationListVM()
        {

        }
        public ConsultationListVM(Consultation_v2 dto)
        {
            Id = dto.Id;
            PatientName = dto.PatientName;
            AppointmentDate = dto.AppointmentDate;
            Symptoms = dto.Symptoms;
            Diagnosis = dto.Diagnosis;
            Notes = dto.Notes;
            AppointmentID = dto.AppointmentID;
            PatientId = dto.PatientId;
        }

        public int Id { get; set; }
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }
        [Required]
        [Display(Name = "Symptoms")]
        public string Symptoms { get; set; }
        [Required]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }
        [Required]
        [Display(Name = "Self-Care Notes")]
        public string Notes { get; set; }
        public int PatientId { get; set; }
        public int AppointmentID { get; set; }
        public virtual Appointment Appointment { get; set; }
    }

    public class CreateCosulationModel
    {
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }
        [Required]
        [Display(Name = "Symptoms")]
        public string Symptoms { get; set; }
        [Required]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }
        [Required]
        [Display(Name = "Self-Care Notes")]
        public string Notes { get; set; }
        public int PatientId { get; set; }
        public int AppointmentID { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}