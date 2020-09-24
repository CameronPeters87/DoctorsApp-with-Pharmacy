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
    public class CustomerOrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CustomerOrders
        public IEnumerable<CustomerOrderModel> GetCustomerOrders()
        {
            List<CustomerOrderModel> orders = new List<CustomerOrderModel>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CustomerOrder, CustomerOrderModel>());
            var mapper = config.CreateMapper();

            foreach (var order in db.CustomerOrders)
            {
                CustomerOrderModel model = mapper.Map<CustomerOrderModel>(order);
                orders.Add(model);
            }

            return orders;
        }

        // GET: api/CustomerOrders/5
        [ResponseType(typeof(CustomerOrder))]
        public async Task<IHttpActionResult> GetCustomerOrder(int id)
        {
            CustomerOrder customerOrder = await db.CustomerOrders.FindAsync(id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<CustomerOrder, CustomerOrderModel>());
            var mapper = config.CreateMapper();

            CustomerOrderModel model = mapper.Map<CustomerOrderModel>(customerOrder);

            return Ok(customerOrder);
        }

        // PUT: api/CustomerOrders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomerOrder(CustomerOrderModel model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CustomerOrderModel, CustomerOrder>());
            var mapper = config.CreateMapper();

            CustomerOrder order = mapper.Map<CustomerOrder>(model);

            db.Entry(order).State = EntityState.Modified;

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

        // POST: api/CustomerOrders
        [ResponseType(typeof(CustomerOrder))]
        public async Task<IHttpActionResult> PostCustomerOrder(CustomerOrder customerOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerOrders.Add(customerOrder);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customerOrder.Id }, customerOrder);
        }

        // DELETE: api/CustomerOrders/5
        [ResponseType(typeof(CustomerOrder))]
        public async Task<IHttpActionResult> DeleteCustomerOrder(int id)
        {
            CustomerOrder customerOrder = await db.CustomerOrders.FindAsync(id);
            if (customerOrder == null)
            {
                return NotFound();
            }

            db.CustomerOrders.Remove(customerOrder);
            await db.SaveChangesAsync();

            return Ok(customerOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerOrderExists(int id)
        {
            return db.CustomerOrders.Count(e => e.Id == id) > 0;
        }
    }
}