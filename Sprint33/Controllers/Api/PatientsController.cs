using AutoMapper;
using Sprint33.ApiModels;
using Sprint33.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sprint33.Controllers.Api
{
    public class PatientsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Patients
        public IEnumerable<PatientModel> GetPatients()
        {
            var patients = (from p in db.Patients
                            select new PatientModel
                            {
                                FirstName = p.FirstName,
                                AddressId = p.AddressId,
                                Age = p.Age,
                                ContactNumber = p.ContactNumber,
                                Email = p.Email,
                                isActive = p.isActive,
                                isLoyal = p.isLoyal,
                                Password = p.Password,
                                Surname = p.Surname,
                                UserID = p.UserID
                            }).ToList();
            return patients;
        }

        // GET: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> GetPatient(int id)
        {
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Patient, PatientModel>());
            var mapper = config.CreateMapper();

            PatientModel model = mapper.Map<PatientModel>(patient);

            return Ok(model);
        }

        // PUT: api/Patients/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.UserID)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> PostPatient(PatientModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<PatientModel, Patient>());
            var mapper = config.CreateMapper();

            var address = new PharmacyEntities.Address
            {
                City = model.Address.City,
                Country = model.Address.Country,
                Province = model.Address.Province,
                Route = model.Address.Route,
                Street_Number = model.Address.Street_Number,
                ZipCode = model.Address.ZipCode
            };

            db.Addresses.Add(address);
            db.SaveChanges();

            var lastAddress = db.Addresses.OrderByDescending(a => a.Id).FirstOrDefault();

            Patient patient = mapper.Map<Patient>(model);
            patient.Address = lastAddress;

            db.Patients.Add(patient);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = patient.UserID }, patient);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof(Patient))]
        public async Task<IHttpActionResult> DeletePatient(int id)
        {
            Patient patient = await db.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            await db.SaveChangesAsync();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.UserID == id) > 0;
        }
    }
}