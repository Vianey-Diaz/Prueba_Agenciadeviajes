using AgenciadeViajesApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgenciadeViajesApi.Controllers
{
    public class TipoHabitacionController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        // GET: api/TipoHabitacion
        /// <summary>
        /// Obtiene la lista completa de tipos de habitación
        /// </summary>
        /// <returns>Lista de todos los tipos de habitación registrados</returns>
        public IEnumerable<TipoHabitacion> Get()
        {
            return db.TiposdeHabitacion;
        }

        // GET: api/TipoHabitacion/5
        /// <summary>
        /// Busca un tipo de habitación por su ID
        /// </summary>
        /// <param name="id">ID del tipo de habitación a buscar</param>
        /// <returns>
        /// Tipo de habitación encontrado o NotFound si no existe
        /// </returns>
        public IHttpActionResult GetBuscar(int id)
        {
            TipoHabitacion tipoHabitacion = db.TiposdeHabitacion.Find(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }
            return Ok(tipoHabitacion);
        }

        // POST: api/TipoHabitacion
        /// <summary>
        /// Crea un nuevo tipo de habitación
        /// </summary>
        /// <param name="tipoHabitacion">Datos del tipo de habitación a crear</param>
        /// <returns>
        /// Tipo de habitación creado con su ID generado
        /// </returns>
        public IHttpActionResult Post(TipoHabitacion tipoHabitacion)
        {
            if (tipoHabitacion == null)
            {
                return BadRequest("El tipo de habitación no puede estar vacío.");
            }

            try
            {
                // Validación de datos
                if (string.IsNullOrWhiteSpace(tipoHabitacion.NombreHabitacion))
                {
                    return BadRequest("El nombre de la habitación es obligatorio.");
                }

                if (tipoHabitacion.PrecioPorNoche <= 0)
                {
                    return BadRequest("El precio por noche debe ser mayor a 0.");
                }

                // Agregar el tipo de habitación a la base de datos
                db.TiposdeHabitacion.Add(tipoHabitacion);
                db.SaveChanges();

                // Retornar el tipo de habitación creado con su ID
                return CreatedAtRoute("DefaultApi", new { id = tipoHabitacion.Id }, tipoHabitacion);
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                return InternalServerError(new Exception("Ocurrió un error inesperado al crear el tipo de habitación.", ex));
            }
        }

        // PUT: api/TipoHabitacion/5
        /// <summary>
        /// Actualiza los datos de un tipo de habitación existente
        /// </summary>
        /// <param name="id">ID del tipo de habitación a modificar</param>
        /// <param name="tipoHabitacion">Datos actualizados del tipo de habitación</param>
        /// <returns>
        /// Tipo de habitación modificado o NotFound si no existe
        /// </returns>
        public IHttpActionResult Put(int id, TipoHabitacion tipoHabitacion)
        {
            if (id != tipoHabitacion.Id)
            {
                return BadRequest("El ID del tipo de habitación no coincide con la URL");
            }

            db.Entry(tipoHabitacion).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(tipoHabitacion);
        }

        // DELETE: api/TipoHabitacion/5
        /// <summary>
        /// Elimina un tipo de habitación por su ID
        /// </summary>
        /// <param name="id">ID del tipo de habitación a eliminar</param>
        /// <returns>
        /// Tipo de habitación eliminado o NotFound si no existe
        /// </returns>
        public IHttpActionResult Delete(int id)
        {
            TipoHabitacion tipoHabitacion = db.TiposdeHabitacion.Find(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            db.TiposdeHabitacion.Remove(tipoHabitacion);
            db.SaveChanges();
            return Ok(tipoHabitacion);
        }
    }

}
