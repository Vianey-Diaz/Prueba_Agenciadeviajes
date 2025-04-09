using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AgenciadeViajesApi.Controllers
{
    public class CotizacionesController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Cotizaciones
        /// <summary>
        /// Obtiene la lista de todas las cotizaciones.
        /// </summary>
        /// <returns>Lista de cotizaciones</returns>
        public IHttpActionResult Get()
        {
            var result = from cotizacion in db.Cotizaciones
                         join cliente in db.Clientes on cotizacion.ClienteId equals cliente.Id
                         join agente in db.AgenteViajes on cotizacion.AgenteResponsableId equals agente.Id
                         join paquete in db.PaqueteTuristicos on cotizacion.PaqueteId equals paquete.Id
                         select new
                         {
                             cotizacion.Id,
                             cotizacion.ClienteId,
                             ClienteNombre = cliente.Nombre,
                             cotizacion.AgenteResponsableId,
                             AgenteNombre = agente.Nombre,
                             cotizacion.PaqueteId,
                             PaqueteNombre = paquete.Nombre,
                             cotizacion.CantidadPersonas,
                             cotizacion.FechaCotizacion,
                             cotizacion.CostoTotal
                         };

            return Ok(result);
        }

        // GET: api/Cotizaciones/5
        /// <summary>
        /// Obtiene una cotización por su ID.
        /// </summary>
        /// <param name="id">ID de la cotización</param>
        /// <returns>Cotización correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var cotizacion = db.Cotizaciones
                .Where(c => c.Id == id)
                .Join(db.Clientes, c => c.ClienteId, cliente => cliente.Id, (c, cliente) => new { c, cliente })
                .Join(db.AgenteViajes, x => x.c.AgenteResponsableId, agente => agente.Id, (x, agente) => new { x, agente })
                .Join(db.PaqueteTuristicos, x => x.x.c.PaqueteId, paquete => paquete.Id, (x, paquete) => new
                {
                    x.x.c.Id,
                    x.x.c.ClienteId,
                    ClienteNombre = x.x.cliente.Nombre,
                    x.x.c.AgenteResponsableId,
                    AgenteNombre = x.agente.Nombre,
                    x.x.c.PaqueteId,
                    PaqueteNombre = paquete.Nombre,
                    x.x.c.CantidadPersonas,
                    x.x.c.FechaCotizacion,
                    x.x.c.CostoTotal
                })
                .FirstOrDefault();

            if (cotizacion == null)
            {
                return NotFound();
            }

            return Ok(cotizacion);
        }

        // POST: api/Cotizaciones
        /// <summary>
        /// Crea una nueva cotización.
        /// </summary>
        /// <param name="cotizacion">Datos de la nueva cotización</param>
        /// <returns>Resultado de la operación de creación</returns>
        public IHttpActionResult Post([FromBody] Cotizacion cotizacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cliente = db.Clientes.Find(cotizacion.ClienteId);
            if (cliente == null)
            {
                return BadRequest("Cliente no válido");
            }

            var agente = db.AgenteViajes.Find(cotizacion.AgenteResponsableId);
            if (agente == null)
            {
                return BadRequest("Agente no válido");
            }

            var paquete = db.PaqueteTuristicos.Find(cotizacion.PaqueteId);
            if (paquete == null)
            {
                return BadRequest("Paquete no válido");
            }

            cotizacion.CostoTotal = cotizacion.Costo();
            cotizacion.FechaCotizacion = DateTime.Now;

            db.Cotizaciones.Add(cotizacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cotizacion.Id }, cotizacion);
        }

        // PUT: api/Cotizaciones/5
        /// <summary>
        /// Actualiza una cotización existente.
        /// </summary>
        /// <param name="id">ID de la cotización a actualizar</param>
        /// <param name="cotizacion">Datos de la cotización actualizada</param>
        /// <returns>Resultado de la operación de actualización</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCotizacion(int id, Cotizacion cotiza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cotiza.Id)
            {
                return BadRequest();
            }

            db.Entry(cotiza).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Cotizacionxists(id))
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

        // DELETE: api/Cotizaciones/5
        /// <summary>
        /// Elimina una cotización por su ID.
        /// </summary>
        /// <param name="id">ID de la cotización a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            var cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            db.Cotizaciones.Remove(cotizacion);
            db.SaveChanges();

            return Ok(cotizacion);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        private bool Cotizacionxists(int id)
        {
            return db.Cotizaciones.Count(e => e.Id == id) > 0;
        }




    }

}
