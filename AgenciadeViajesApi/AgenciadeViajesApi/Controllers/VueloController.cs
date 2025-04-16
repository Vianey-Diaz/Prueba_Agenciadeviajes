using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AgenciadeViajesApi.Controllers
{

    public class VueloController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Transporte
        /// <summary>
        /// Obtiene todos los transportes registrados
        /// </summary>
        public IHttpActionResult GetVuelos()
        {
            var vuelos = from vuelo in db.Vuelos
                         join origen in db.Destinos on vuelo.OrigenId equals origen.Id
                         join destino in db.Destinos on vuelo.DestinoId equals destino.Id
                         select new
                         {
                             Id = vuelo.Id,
                             Nombre = vuelo.Nombre,
                             Tipo = vuelo.Tipo,
                             Compañia = vuelo.Compañia,
                             HoraSalida = vuelo.HoraSalida,
                             HoraLlegada = vuelo.HoraLlegada,
                             Capacidad = vuelo.Capacidad,
                             Precio = vuelo.Precio,
                             OrigenId = origen.Id,
                             DestinoId = destino.Id,
                             OrigenNombre = origen.NomDestino,
                             DestinoNombre = destino.NomDestino
                         };

            if (!vuelos.Any())
            {
                return NotFound();
            }

            return Ok(vuelos);
        }

        // GET: api/Transporte/5
        /// <summary>
        /// Busca transporte por ID
        /// </summary>
        public IHttpActionResult GetVuelo(int id)
        {
            var vuelo = from v in db.Vuelos
                        join origen in db.Destinos on v.OrigenId equals origen.Id
                        join destino in db.Destinos on v.DestinoId equals destino.Id
                        select new
                        {
                            v.Id,
                            v.Nombre,
                            v.Tipo,
                            v.Compañia,
                            v.HoraSalida,
                            v.HoraLlegada,
                            v.Capacidad,
                            v.Precio,
                            OrigenNombre = origen.NomDestino,
                            DestinoNombre = destino.NomDestino
                        };

            if (vuelo == null)
            {
                return NotFound();
            }

            return Ok(vuelo);
        }

        // POST: api/Transporte
        /// <summary>
        /// Crea nuevo transporte
        /// </summary>
        public IHttpActionResult Post([FromBody] Vuelo vuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que el origen exista
            var origen = db.Destinos.Find(vuelo.OrigenId);
            if (origen == null)
            {
                return BadRequest("Origen no válido");
            }

            // Validar que el destino exista
            var destino = db.Destinos.Find(vuelo.DestinoId);
            if (destino == null)
            {
                return BadRequest("Destino no válido");
            }

            // Asignar las relaciones de navegación (opcional, útil si las usas después)
            vuelo.Origen = origen;
            vuelo.Destino = destino;

            db.Vuelos.Add(vuelo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vuelo.Id }, vuelo);
        }

        // PUT: api/Transporte/5
        /// <summary>
        /// Actualiza transporte existente
        /// </summary>
        public IHttpActionResult Put(int id, [FromBody] Vuelo vuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vueloExistente = db.Vuelos.Find(id);
            if (vueloExistente == null)
            {
                return NotFound();
            }

            var origen = db.Destinos.Find(vuelo.OrigenId);
            if (origen == null)
            {
                return BadRequest("Origen no válido");
            }

            var destino = db.Destinos.Find(vuelo.DestinoId);
            if (destino == null)
            {
                return BadRequest("Destino no válido");
            }

            // Actualizar propiedades
            vueloExistente.Nombre = vuelo.Nombre;
            vueloExistente.Tipo = vuelo.Tipo;
            vueloExistente.Compañia = vuelo.Compañia;
            vueloExistente.HoraSalida = vuelo.HoraSalida;
            vueloExistente.HoraLlegada = vuelo.HoraLlegada;
            vueloExistente.Capacidad = vuelo.Capacidad;
            vueloExistente.Precio = vuelo.Precio;
            vueloExistente.OrigenId = vuelo.OrigenId;
            vueloExistente.DestinoId = vuelo.DestinoId;

            db.SaveChanges();

            return Ok(vueloExistente);
        }

        // DELETE: api/Transporte/5
        /// <summary>
        /// Elimina transporte por ID
        /// </summary>
        public IHttpActionResult Delete(int id)
        {
            Vuelo transporte = db.Vuelos.Find(id);
            if (transporte == null) return NotFound();

            db.Vuelos.Remove(transporte);
            db.SaveChanges();
            return Ok(transporte);
        }


    }
}
