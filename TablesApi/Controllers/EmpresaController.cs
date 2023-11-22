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
    public class EmpresaController : ApiController
    {
        private PGEntities db = new PGEntities();

        // GET: api/Empresa
        public IQueryable<Empresa> GetEmpresa()
        {
            return db.Empresa;
        }

        // GET: api/Empresa/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult GetEmpresa(int id)
        {
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }

            return Ok(empresa);
        }

        // PUT: api/Empresa/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmpresa(int id, Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresa.IdEmpresa)
            {
                return BadRequest();
            }

            db.Entry(empresa).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
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

        // POST: api/Empresa
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult PostEmpresa(Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Empresa.Add(empresa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = empresa.IdEmpresa }, empresa);
        }

        // DELETE: api/Empresa/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult DeleteEmpresa(int id)
        {
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return NotFound();
            }

            db.Empresa.Remove(empresa);
            db.SaveChanges();

            return Ok(empresa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpresaExists(int id)
        {
            return db.Empresa.Count(e => e.IdEmpresa == id) > 0;
        }
    }
}