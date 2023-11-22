using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TablesApi.Models;

namespace TablesApi.Controllers
{
    [Authorize]
    public class ProductoController : ApiController
    {
        private PGEntities db = new PGEntities();

        // GET: api/Producto
        public IQueryable<Producto> GetProducto()
        {
            return db.Producto;
        }

        // GET: api/Producto/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult GetProducto(string id)
        {
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // PUT: api/Producto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducto(string id, Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.Idproducto)
            {
                return BadRequest();
            }

            db.Entry(producto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // POST: api/Producto
        [ResponseType(typeof(Producto))]
        public IHttpActionResult PostProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Producto.Add(producto);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProductoExists(producto.Idproducto))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = producto.Idproducto }, producto);
        }

        // DELETE: api/Producto/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult DeleteProducto(string id)
        {
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            db.Producto.Remove(producto);
            db.SaveChanges();

            return Ok(producto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductoExists(string id)
        {
            return db.Producto.Count(e => e.Idproducto == id) > 0;
        }
    }
}