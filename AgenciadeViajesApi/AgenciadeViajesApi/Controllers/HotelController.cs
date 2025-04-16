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
    public class HotelController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/Hotel
        /// <summary>
        /// Obtiene todos los hoteles registrados
        /// </summary>
        /// <returns>Lista de todos los hoteles</returns>
        public IHttpActionResult Get()
        {
            var result = from hotel in db.Hotel
                         join tipo in db.TiposdeHabitacion on hotel.Tipohabitacion.Id equals tipo.Id
                         select new
                         {
                             Id = hotel.Id,
                             Nombre = hotel.Nombre,
                             Estrellas = hotel.Estrellas,
                             Direccion = hotel.Direccion,
                             TipoHabitacionId = tipo.Id,
                             NombreHabitacion = tipo.NombreHabitacion
                         };

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Hotel/5
        /// <summary>
        /// Busca un hotel por su ID
        /// </summary>
        /// <returns>lista de hotel por id</returns>
        public IHttpActionResult GetBuscar(int id)
        {
            Hotel hotel = db.Hotel.Find(id);
            if (hotel == null) return NotFound();
            return Ok(hotel);
        }

        // POST: api/Hotel
        /// <summary>
        /// Crea un nuevo hotel
        /// </summary>
        /// <returns>Agrega un hotel</returns>
        // POST: api/Hotel
        /// <summary>
        /// Crea un nuevo hotel.
        /// </summary>
        /// <param name="hotel">Datos del hotel a crear</param>
        /// <returns>Resultado de la operación de creación</returns>
        public IHttpActionResult Post([FromBody] Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (hotel.Tipohabitacion == null || hotel.Tipohabitacion.Id == 0)
            {
                return BadRequest("Tipo de habitación no válido o no enviado correctamente.");
            }

            var tipo = db.TiposdeHabitacion.Find(hotel.Tipohabitacion.Id);
            if (tipo == null)
            {
                return BadRequest("Tipo de habitación no encontrado en la base de datos.");
            }

            hotel.Tipohabitacion = tipo;

            db.Hotel.Add(hotel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hotel.Id }, hotel);
        }

        // PUT: api/Hotel/5
        /// <summary>
        /// Actualiza un hotel existente.
        /// </summary>
        /// <param name="id">ID del hotel a actualizar</param>
        /// <param name="hotel">Datos actualizados del hotel</param>
        /// <returns>Resultado de la operación de actualización</returns>
        public IHttpActionResult Put(int id, [FromBody] Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotel.Id)
            {
                return BadRequest("IDs no coinciden");
            }

            var hotelExistente = db.Hotel.Find(id);
            if (hotelExistente == null)
            {
                return NotFound();
            }

            var tipoHabitacion = db.TiposdeHabitacion.Find(hotel.Tipohabitacion.Id);
            if (tipoHabitacion == null)
            {
                return BadRequest("Tipo de habitación no válido");
            }

            // Actualizar propiedades del hotel
            hotelExistente.Nombre = hotel.Nombre;
            hotelExistente.Tipohabitacion = tipoHabitacion;
            hotelExistente.Estrellas = hotel.Estrellas;
            hotelExistente.Direccion = hotel.Direccion;

            db.SaveChanges();

            return Ok(hotelExistente);
        }

        // DELETE: api/Hotel/5
        /// <summary>
        /// Elimina un hotel por su ID.
        /// </summary>
        /// <param name="id">ID del hotel a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            var hotel = db.Hotel.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }

            db.Hotel.Remove(hotel);
            db.SaveChanges();

            return Ok(hotel);
        }

    }
}

