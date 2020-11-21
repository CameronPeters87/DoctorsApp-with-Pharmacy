using AutoMapper;
using Sprint33.ApiModels;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
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
    public class DriversController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Drivers
        public IEnumerable<DriverApiModel> GetDrivers()
        {
            List<DriverApiModel> drivers = new List<DriverApiModel>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Driver, DriverApiModel>());
            var mapper = config.CreateMapper();

            foreach (var driver in db.Drivers)
            {
                DriverApiModel model = mapper.Map<DriverApiModel>(driver);
                drivers.Add(model);
            }

            return drivers;
        }

        // GET: api/Drivers/5
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> GetDriver(int id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Driver, DriverApiModel>());
            var mapper = config.CreateMapper();

            DriverApiModel model = mapper.Map<DriverApiModel>(driver);

            return Ok(driver);
        }

        // PUT: api/Drivers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDriver(DriverApiModel model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DriverApiModel, Driver>());
            var mapper = config.CreateMapper();

            Driver driver = mapper.Map<Driver>(model);

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Drivers
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> PostDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = driver.Id }, driver);
        }

        // DELETE: api/Drivers/5
        [ResponseType(typeof(Driver))]
        public async Task<IHttpActionResult> DeleteDriver(int id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();

            return Ok(driver);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.Id == id) > 0;
        }
    }
}