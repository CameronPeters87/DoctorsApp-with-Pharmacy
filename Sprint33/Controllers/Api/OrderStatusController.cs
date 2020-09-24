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
    public class OrderStatusController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OrderStatus
        public IEnumerable<OrderStatusModel> GetOrderStatuses()
        {
            List<OrderStatusModel> statusList = new List<OrderStatusModel>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderStatus, OrderStatusModel>());
            var mapper = config.CreateMapper();

            foreach (var status in db.OrderStatuses)
            {
                OrderStatusModel model = mapper.Map<OrderStatusModel>(status);
                statusList.Add(model);
            }

            return statusList;
        }

        // GET: api/OrderStatus/5
        [ResponseType(typeof(OrderStatus))]
        public async Task<IHttpActionResult> GetOrderStatus(int id)
        {
            OrderStatus orderStatus = await db.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            return Ok(orderStatus);
        }

        // PUT: api/OrderStatus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrderStatus(int id, OrderStatus orderStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(orderStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusExists(id))
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

        // POST: api/OrderStatus
        [ResponseType(typeof(OrderStatus))]
        public async Task<IHttpActionResult> PostOrderStatus(OrderStatus orderStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderStatuses.Add(orderStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orderStatus.Id }, orderStatus);
        }

        // DELETE: api/OrderStatus/5
        [ResponseType(typeof(OrderStatus))]
        public async Task<IHttpActionResult> DeleteOrderStatus(int id)
        {
            OrderStatus orderStatus = await db.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            db.OrderStatuses.Remove(orderStatus);
            await db.SaveChangesAsync();

            return Ok(orderStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderStatusExists(int id)
        {
            return db.OrderStatuses.Count(e => e.Id == id) > 0;
        }
    }
}