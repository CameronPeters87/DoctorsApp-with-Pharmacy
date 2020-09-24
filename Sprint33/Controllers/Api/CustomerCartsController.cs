using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sprint33.Models;
using Sprint33.PharmacyEntities;

namespace Sprint33.Controllers.Api
{
    public class CustomerCartsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CustomerCarts
        public IQueryable<CustomerCart> GetCustomerCarts()
        {
            return db.CustomerCarts;
        }

        // GET: api/CustomerCarts/5
        [ResponseType(typeof(CustomerCart))]
        public async Task<IHttpActionResult> GetCustomerCart(int id)
        {
            CustomerCart customerCart = await db.CustomerCarts.FindAsync(id);
            if (customerCart == null)
            {
                return NotFound();
            }

            return Ok(customerCart);
        }

        // PUT: api/CustomerCarts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomerCart(int id, CustomerCart customerCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerCart.Id)
            {
                return BadRequest();
            }

            db.Entry(customerCart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerCartExists(id))
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

        // POST: api/CustomerCarts
        [ResponseType(typeof(CustomerCart))]
        public async Task<IHttpActionResult> PostCustomerCart(CustomerCart customerCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerCarts.Add(customerCart);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customerCart.Id }, customerCart);
        }

        // DELETE: api/CustomerCarts/5
        [ResponseType(typeof(CustomerCart))]
        public async Task<IHttpActionResult> DeleteCustomerCart(int id)
        {
            CustomerCart customerCart = await db.CustomerCarts.FindAsync(id);
            if (customerCart == null)
            {
                return NotFound();
            }

            db.CustomerCarts.Remove(customerCart);
            await db.SaveChangesAsync();

            return Ok(customerCart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerCartExists(int id)
        {
            return db.CustomerCarts.Count(e => e.Id == id) > 0;
        }
    }
}