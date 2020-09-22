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
    public class InStoreSalesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/InStoreSales
        public IQueryable<InStoreSale> GetInStoreSales()
        {
            return db.InStoreSales;
        }

        // GET: api/InStoreSales/5
        [ResponseType(typeof(InStoreSale))]
        public async Task<IHttpActionResult> GetInStoreSale(int id)
        {
            InStoreSale inStoreSale = await db.InStoreSales.FindAsync(id);
            if (inStoreSale == null)
            {
                return NotFound();
            }

            return Ok(inStoreSale);
        }

        // PUT: api/InStoreSales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInStoreSale(int id, InStoreSale inStoreSale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inStoreSale.Id)
            {
                return BadRequest();
            }

            db.Entry(inStoreSale).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InStoreSaleExists(id))
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

        // POST: api/InStoreSales
        [ResponseType(typeof(InStoreSale))]
        public async Task<IHttpActionResult> PostInStoreSale(InStoreSale inStoreSale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InStoreSales.Add(inStoreSale);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = inStoreSale.Id }, inStoreSale);
        }

        // DELETE: api/InStoreSales/5
        [ResponseType(typeof(InStoreSale))]
        public async Task<IHttpActionResult> DeleteInStoreSale(int id)
        {
            InStoreSale inStoreSale = await db.InStoreSales.FindAsync(id);
            if (inStoreSale == null)
            {
                return NotFound();
            }

            db.InStoreSales.Remove(inStoreSale);
            await db.SaveChangesAsync();

            return Ok(inStoreSale);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InStoreSaleExists(int id)
        {
            return db.InStoreSales.Count(e => e.Id == id) > 0;
        }
    }
}