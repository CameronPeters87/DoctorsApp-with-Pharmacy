using System.Collections.Generic;

namespace Sprint33.ApiModels
{
    public class MainModel
    {
        public ICollection<AppointmentModel> AppointmentModels { get; set; }
        public ICollection<InStoreSaleModel> InStoreSaleModels { get; set; }
        public ICollection<PatientModel> PatientModels { get; set; }
        public ICollection<ProductModel> ProductModels { get; set; }
    }
}