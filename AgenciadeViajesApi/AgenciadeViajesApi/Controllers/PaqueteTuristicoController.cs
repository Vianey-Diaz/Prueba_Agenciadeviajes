using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace AgenciadeViajesApi.Controllers
{
   
    public class PaquetesTuristicosController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/PaquetesTuristicos
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Get()
        {
            var result = from paquete in db.PaqueteTuristicos
                         join vuelo in db.Vuelos on paquete.VueloId equals vuelo.Id into vuelos
                         from vuelo in vuelos.DefaultIfEmpty()
                         join hotel in db.Hotel on paquete.HotelId equals hotel.Id into hoteles
                         from hotel in hoteles.DefaultIfEmpty()
                         join actividad in db.Actividades on paquete.ActividadesId equals actividad.Id into actividades
                         from actividad in actividades.DefaultIfEmpty()
                         join guia in db.GuiaTuristicos on paquete.GuiaTuristicoId equals guia.Id into guias
                         from guia in guias.DefaultIfEmpty()
                         join seguro in db.Seguros on paquete.SeguroId equals seguro.Id into seguros
                         from seguro in seguros.DefaultIfEmpty()
                         select new
                         {
                             Id = paquete.Id,
                             Nombre = paquete.Nombre,
                             DestinoId = paquete.DestinoId,
                             VueloId = vuelo != null ? vuelo.Id : (int?)null,
                             HotelId = hotel != null ? hotel.Id : (int?)null,
                             ActividadesId = actividad != null ? actividad.Id : (int?)null,
                             GuiaTuristicoId = guia != null ? guia.Id : (int?)null,
                             SeguroId = seguro != null ? seguro.Id : (int?)null,
                             PrecioTotal = paquete.PrecioTotal,
                             FechaExpiracion = paquete.FechaExpiracion,
                             Estado = paquete.Estado,
                             Duracion_Dias = paquete.Duracion_Dias
                         };

            return Ok(result);
        }

        // GET: api/PaquetesTuristicos/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Paquete_Turistico))]
        public IHttpActionResult Get(int id)
        {
            var paquete = from p in db.PaqueteTuristicos
                          where p.Id == id
                          join vuelo in db.Vuelos on p.VueloId equals vuelo.Id into vuelos
                          from vuelo in vuelos.DefaultIfEmpty()
                          join hotel in db.Hotel on p.HotelId equals hotel.Id into hoteles
                          from hotel in hoteles.DefaultIfEmpty()
                          join actividad in db.Actividades on p.ActividadesId equals actividad.Id into actividades
                          from actividad in actividades.DefaultIfEmpty()
                          join guia in db.GuiaTuristicos on p.GuiaTuristicoId equals guia.Id into guias
                          from guia in guias.DefaultIfEmpty()
                          join seguro in db.Seguros on p.SeguroId equals seguro.Id into seguros
                          from seguro in seguros.DefaultIfEmpty()
                          select new
                          {
                              Id = p.Id,
                              Nombre_paquete = p.Nombre,
                              DestinoId = p.DestinoId,
                              VueloId = vuelo != null ? vuelo.Id : (int?)null,
                              HotelId = hotel != null ? hotel.Id : (int?)null,
                              ActividadesId = actividad != null ? actividad.Id : (int?)null,
                              GuiaTuristicoId = guia != null ? guia.Id : (int?)null,
                              SeguroId = seguro != null ? seguro.Id : (int?)null,
                              PrecioTotal = p.PrecioTotal,
                              FechaExpiracion = p.FechaExpiracion,
                              Estado = p.Estado,
                              Duracion_Dias = p.Duracion_Dias
                          };

            if (!paquete.Any())
            {
                return NotFound();
            }

            return Ok(paquete);
        }

        // POST: api/PaquetesTuristicos
        [ResponseType(typeof(Paquete_Turistico))]
        public IHttpActionResult Post(Paquete_Turistico paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (paquete.DestinoId == 0)
            {
                return BadRequest("El campo DestinoId es requerido");
            }

            var destino = db.Destinos.Find(paquete.DestinoId);
            if (destino == null)
            {
                return BadRequest("Destino no encontrado");
            }

            if (paquete.VueloId.HasValue && db.Vuelos.Find(paquete.VueloId.Value) == null)
            {
                return BadRequest("Vuelo no encontrado");
            }

            if (paquete.HotelId.HasValue && db.Hotel.Find(paquete.HotelId.Value) == null)
            {
                return BadRequest("Hotel no encontrado");
            }

            if (paquete.SeguroId.HasValue && db.Seguros.Find(paquete.SeguroId.Value) == null)
            {
                return BadRequest("Seguro no encontrado");
            }

            if (paquete.GuiaTuristicoId.HasValue && db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) == null)
            {
                return BadRequest("Guía turístico no encontrado");
            }

            if (paquete.ActividadesId.HasValue && db.Actividades.Find(paquete.ActividadesId.Value) == null)
            {
                return BadRequest("Actividad no encontrada");
            }

            paquete.Vuelo = paquete.VueloId.HasValue ? db.Vuelos.Find(paquete.VueloId.Value) : null;
            paquete.Hotel = paquete.HotelId.HasValue ? db.Hotel.Find(paquete.HotelId.Value) : null;
            paquete.Seguro = paquete.SeguroId.HasValue ? db.Seguros.Find(paquete.SeguroId.Value) : null;
            paquete.GuiaTuristico = paquete.GuiaTuristicoId.HasValue ? db.GuiaTuristicos.Find(paquete.GuiaTuristicoId.Value) : null;
            paquete.Actividades = paquete.ActividadesId.HasValue ? db.Actividades.Find(paquete.ActividadesId.Value) : null;
            paquete.Destino = destino;

            paquete.PrecioTotal = paquete.CalcularCostoTotal();

            db.PaqueteTuristicos.Add(paquete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paquete.Id }, paquete);
        }

        // PUT: api/PaquetesTuristicos/5
        /// <summary>
        /// actualiza datos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paquete"></param>
        /// <returns></returns>

       
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPaqueteTuristico(int id, Paquete_Turistico paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paquete.Id)
            {
                return BadRequest();
            }

            db.Entry(paquete).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaqueteTuristicoExists(id))
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

        private bool PaqueteTuristicoExists(int id)
        {
            return db.PaqueteTuristicos.Count(e => e.Id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





        /*
        [System.Web.Http.HttpPut]
        public IHttpActionResult PutPaqueteTuristico(int id, Paquete_Turistico paquete)
        {
            if (id != paquete.Id)
                return BadRequest("El ID en la URL no coincide con el ID del paquete.");

            var paqueteExistente = db.PaqueteTuristicos.Include("Vuelo")
                                                        .Include("Hotel.Tipohabitacion")
                                                        .Include("Destino")
                                                        .Include("Seguro")
                                                        .Include("GuiaTuristico")
                                                        .Include("Actividades")
                                                        .FirstOrDefault(p => p.Id == id);

            if (paqueteExistente == null)
                return NotFound();

            // Actualizar datos simples
            paqueteExistente.Nombre = paquete.Nombre;
            paqueteExistente.FechaExpiracion = paquete.FechaExpiracion;
            paqueteExistente.Duracion_Dias = paquete.Duracion_Dias;
            paqueteExistente.Estado = paquete.Estado;

            // Reasignar relaciones con base en los IDs
            paqueteExistente.Vuelo = db.Vuelos.Find(paquete.VueloId);
            paqueteExistente.Hotel = db.Hotel.Include("Tipohabitacion").FirstOrDefault(h => h.Id == paquete.HotelId);
            paqueteExistente.Destino = db.Destinos.Find(paquete.DestinoId);
            paqueteExistente.Seguro = db.Seguros.Find(paquete.SeguroId);
            paqueteExistente.GuiaTuristico = db.GuiaTuristicos.Find(paquete.GuiaTuristicoId);
            paqueteExistente.Actividades = db.Actividades.Find(paquete.ActividadesId);

            // Calcular costo total actualizado
            paqueteExistente.PrecioTotal = paqueteExistente.CalcularCostoTotal();

            db.Entry(paqueteExistente).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new { mensaje = "Paquete actualizado correctamente." });
        }
        */




        private bool PaqueteExists(int id)
        {
            return db.PaqueteTuristicos.Any(p => p.Id == id);
        }


        // DELETE: api/PaquetesTuristicos/5
        [ResponseType(typeof(Paquete_Turistico))]
        public IHttpActionResult Delete(int id)
        {
            Paquete_Turistico paquete = db.PaqueteTuristicos.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            db.PaqueteTuristicos.Remove(paquete);
            db.SaveChanges();

            return Ok(paquete);
        }


        /// <summary>
        /// Busca paquetes turísticos por destino y precio máximo.
        /// </summary>
        /// <param name="destinoId">ID del destino</param>
        /// <param name="precioMaximo">Precio máximo</param>
        /// <returns>Lista de paquetes turísticos que cumplen con los criterios</returns>
        [HttpGet]
        [Route("api/PaqueteTuristico/buscar-por-destino-precio")]
        public IHttpActionResult BuscarPorDestinoYPrecio(int destinoId, double precioMaximo)
        {
            if (destinoId <= 0)
            {
                return BadRequest("El ID del destino debe ser mayor que cero.");
            }

            if (precioMaximo <= 0)
            {
                return BadRequest("El precio máximo debe ser mayor que cero.");
            }

            var resultados = from p in db.PaqueteTuristicos
                             where p.Destino != null && p.Destino.Id == destinoId && p.PrecioTotal <= precioMaximo
                             orderby p.PrecioTotal
                             select new
                             {
                                 p.Id,
                                 p.Nombre,
                                 Destino = p.Destino != null ? p.Destino.NomDestino : "Destino no especificado",
                                 p.PrecioTotal,
                                 Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                                 Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                                 Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                             };

            if (!resultados.Any())
            {
                return NotFound();
            }

            return Ok(resultados);
        }

        /// <summary>
        /// Busca paquetes turísticos por nombre y precio mínimo.
        /// </summary>
        /// <param name="nombre">Nombre del paquete turístico</param>
        /// <param name="precioMinimo">Precio mínimo</param>
        /// <returns>Lista de paquetes turísticos que cumplen con los criterios</returns>
        [HttpGet]
        [Route("api/PaqueteTuristico/buscar-por-nombre-precio")]
        public IHttpActionResult BuscarPorNombreYPrecio(string nombre, double precioMinimo)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            if (precioMinimo <= 0)
            {
                return BadRequest("El precio mínimo debe ser mayor que cero.");
            }

            var resultados = from p in db.PaqueteTuristicos
                             where p.Nombre.Contains(nombre) && p.PrecioTotal >= precioMinimo
                             orderby p.PrecioTotal
                             select new
                             {
                                 p.Id,
                                 p.Nombre,
                                 p.PrecioTotal,
                                 Vuelo = p.Vuelo != null ? p.Vuelo.Compañia : "Sin vuelo",
                                 Hotel = p.Hotel != null ? p.Hotel.Nombre : "Sin hotel",
                                 Seguro = p.Seguro != null ? p.Seguro.Tipo : "Sin seguro"
                             };

            if (!resultados.Any())
            {
                return NotFound();
            }

            return Ok(resultados);
        }


    }

}
