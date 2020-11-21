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
    public class DeliveriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Deliveries
        public IEnumerable<DeliveryApiModel> GetDeliveries()
        {
            List<DeliveryApiModel> deliveries = new List<DeliveryApiModel>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Delivery, DeliveryApiModel>());
            var mapper = config.CreateMapper();

            foreach (var delivery in db.Deliveries)
            {
                DeliveryApiModel model = mapper.Map<DeliveryApiModel>(delivery);
                deliveries.Add(model);
            }

            return deliveries;
        }

        // GET: api/Deliveries/5
        [ResponseType(typeof(Delivery))]
        public async Task<IHttpActionResult> GetDelivery(int id)
        {
            Delivery delivery = await db.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Delivery, DeliveryApiModel>());
            var mapper = config.CreateMapper();

            DeliveryApiModel model = mapper.Map<DeliveryApiModel>(delivery);

            return Ok(delivery);
        }

        // PUT: api/Deliveries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDelivery(DeliveryApiModel model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DeliveryApiModel, Delivery>());
            var mapper = config.CreateMapper();

            Delivery delivery = mapper.Map<Delivery>(model);

            db.Entry(delivery).State = EntityState.Modified;

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

        // POST: api/Deliveries
        [ResponseType(typeof(Delivery))]
        public async Task<IHttpActionResult> PostDelivery(Delivery delivery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Deliveries.Add(delivery);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = delivery.Id }, delivery);
        }

        // DELETE: api/Deliveries/5
        [ResponseType(typeof(Delivery))]
        public async Task<IHttpActionResult> DeleteDelivery(int id)
        {
            Delivery delivery = await db.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            db.Deliveries.Remove(delivery);
            await db.SaveChangesAsync();

            return Ok(delivery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryExists(int id)
        {
            return db.Deliveries.Count(e => e.Id == id) > 0;
        }
    }
}