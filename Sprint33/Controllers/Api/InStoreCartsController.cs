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
    public class InStoreCartsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/InStoreCarts
        public IQueryable<InStoreCart> GetInStoreCarts()
        {
            return db.InStoreCarts;
        }

        // GET: api/InStoreCarts/5
        [ResponseType(typeof(InStoreCart))]
        public async Task<IHttpActionResult> GetInStoreCart(int id)
        {
            InStoreCart inStoreCart = await db.InStoreCarts.FindAsync(id);
            if (inStoreCart == null)
            {
                return NotFound();
            }

            return Ok(inStoreCart);
        }

        // PUT: api/InStoreCarts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInStoreCart(int id, InStoreCart inStoreCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inStoreCart.Id)
            {
                return BadRequest();
            }

            db.Entry(inStoreCart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InStoreCartExists(id))
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

        // POST: api/InStoreCarts
        [ResponseType(typeof(InStoreCart))]
        public async Task<IHttpActionResult> PostInStoreCart(InStoreCart inStoreCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InStoreCarts.Add(inStoreCart);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = inStoreCart.Id }, inStoreCart);
        }

        // DELETE: api/InStoreCarts/5
        [ResponseType(typeof(InStoreCart))]
        public async Task<IHttpActionResult> DeleteInStoreCart(int id)
        {
            InStoreCart inStoreCart = await db.InStoreCarts.FindAsync(id);
            if (inStoreCart == null)
            {
                return NotFound();
            }

            db.InStoreCarts.Remove(inStoreCart);
            await db.SaveChangesAsync();

            return Ok(inStoreCart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InStoreCartExists(int id)
        {
            return db.InStoreCarts.Count(e => e.Id == id) > 0;
        }
    }
}