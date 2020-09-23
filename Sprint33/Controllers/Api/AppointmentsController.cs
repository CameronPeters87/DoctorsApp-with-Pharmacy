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
    public class AppointmentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Appointments
        public IEnumerable<AppointmentModel> GetAppointments()
        {
            List<AppointmentModel> appointmentModels = new List<AppointmentModel>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Appointment, AppointmentModel>());
            var mapper = config.CreateMapper();

            foreach (var appointment in db.Appointments)
            {
                AppointmentModel model = mapper.Map<AppointmentModel>(appointment);
                appointmentModels.Add(model);
            }

            return appointmentModels;
        }

        // GET: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> GetAppointment(int id)
        {
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Appointment, AppointmentModel>());
            var mapper = config.CreateMapper();

            AppointmentModel model = mapper.Map<AppointmentModel>(appointment);

            return Ok(model);
        }

        // PUT: api/Appointments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAppointment(AppointmentModel model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<AppointmentModel, Appointment>());
            var mapper = config.CreateMapper();

            Appointment appointment = mapper.Map<Appointment>(model);

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {

                throw e;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Appointments
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> PostAppointment(AppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<AppointmentModel, Appointment>());
            var mapper = config.CreateMapper();

            Appointment appointment = mapper.Map<Appointment>(model);

            appointment.Patient = db.Patients.Find(model.PatientID);

            db.Appointments.Add(appointment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentID }, appointment);
        }

        // DELETE: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public async Task<IHttpActionResult> DeleteAppointment(int id)
        {
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            await db.SaveChangesAsync();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }
    }
}