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
    public class ReservacionesController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        /// <summary>
        /// Obtiene la lista de todas las reservaciones.
        /// </summary>
        /// <returns>Lista de reservaciones</returns>
        public IHttpActionResult Get()
        {
            var result = from reserva in db.Reservas
                         join cotizacion in db.Cotizaciones on reserva.IdCotizacion equals cotizacion.Id
                         join cliente in db.Clientes on cotizacion.ClienteId equals cliente.Id
                         select new
                         {
                             Id = reserva.Id,
                             IdCotizacion = cotizacion.Id,
                             ClienteNombre = cliente.Nombre,
                             FechaReservacion = reserva.FechaReservacion,
                             Estado = reserva.Estado,
                             FechaViaje = reserva.FechaViaje,
                             FechaRegreso = reserva.FechaRegreso,
                             MontoPagado = reserva.MontoPagado,
                             Saldopendiente = cotizacion.CostoTotal - reserva.MontoPagado
                         };


            return Ok(result);
        }


        /// <summary>
        /// Obtiene una reservación por su ID.
        /// </summary>
        /// <param name="id">ID de la reservación</param>
        /// <returns>Reservación correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var reservacion = db.Reservas
                                .Where(R => R.Id == id)
                                .Select(R => new
                                {
                                    R.Id,
                                    CotizacionId = R.Cotizacion.Id,
                                    FechaReservacion = R.FechaReservacion,
                                    Estado = R.Estado,
                                    FechaViaje = R.FechaViaje,
                                    FechaRegreso = R.FechaRegreso,
                                    MontoPagado = R.MontoPagado,
                                    SaldoPendiente = R.Saldopendiente
                                })
                                .FirstOrDefault();

            if (reservacion == null)
            {
                return NotFound();
            }

            return Ok(reservacion);
        }

        /// <summary>
        /// Crea una nueva reservación.
        /// </summary>
        /// <param name="reservacion">Datos de la nueva reservación</param>
        /// <returns>Resultado de la operación de creación</returns>
        public IHttpActionResult Post(Reservacion reservacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si la cotización existe
                var cotizacion = db.Cotizaciones.Find(reservacion.IdCotizacion);
                if (cotizacion == null)
                {
                    return BadRequest("Cotización no encontrada.");
                }

                // Asignar la cotización
                reservacion.Cotizacion = cotizacion;

                // Calcular el saldo pendiente
                reservacion.CalcularSaldoPendiente();

                db.Reservas.Add(reservacion);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = reservacion.Id }, reservacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una reservación existente.
        /// </summary>
        /// <param name="id">ID de la reservación a actualizar</param>
        /// <param name="reservacion">Datos de la reservación actualizada</param>
        /// <returns>Resultado de la operación de actualización</returns>
        public IHttpActionResult Put(int id, Reservacion reservacion)
        {
            try
            {
                if (id != reservacion.Id)
                {
                    return BadRequest("ID no coincide.");
                }

                var existente = db.Reservas.Find(id);
                if (existente == null)
                {
                    return NotFound();
                }

                // Actualizar los campos de la reservación
                existente.FechaReservacion = reservacion.FechaReservacion;
                existente.Estado = reservacion.Estado;
                existente.FechaViaje = reservacion.FechaViaje;
                existente.FechaRegreso = reservacion.FechaRegreso;
                existente.MontoPagado = reservacion.MontoPagado;

                // Calcular el nuevo saldo pendiente
                existente.CalcularSaldoPendiente();

                db.Entry(existente).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(existente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina una reservación por su ID.
        /// </summary>
        /// <param name="id">ID de la reservación a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            var reservacion = db.Reservas.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            db.Reservas.Remove(reservacion);
            db.SaveChanges();
            return Ok(reservacion);
        }




        /*
        /// <summary>
        /// Obtiene la lista de todas las reservaciones.
        /// </summary>
        /// <returns>Lista de reservaciones</returns>
        public IHttpActionResult Get()
        {
            var reservaciones = from R in db.Reservas
                                select new
                                {
                                    R.Id,
                                    CotizacionId = R.Cotizacion.Id,
                                    FechaReservacion = R.FechaReservacion,
                                    Estado = R.Estado,
                                    FechaViaje = R.FechaViaje,
                                    FechaRegreso = R.FechaRegreso,
                                    MontoPagado = R.MontoPagado,
                                    SaldoPendiente = R.Saldopendiente
                                };

            if (!reservaciones.Any())
            {
                return NotFound();
            }

            return Ok(reservaciones);
        }

        /// <summary>
        /// Obtiene una reservación por su ID.
        /// </summary>
        /// <param name="id">ID de la reservación</param>
        /// <returns>Reservación correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var reservacion = db.Reservas
                                .Where(R => R.Id == id)
                                .Select(R => new
                                {
                                    R.Id,
                                    CotizacionId = R.Cotizacion.Id,
                                    FechaReservacion = R.FechaReservacion,
                                    Estado = R.Estado,
                                    FechaViaje = R.FechaViaje,
                                    FechaRegreso = R.FechaRegreso,
                                    MontoPagado = R.MontoPagado,
                                    SaldoPendiente = R.Saldopendiente
                                })
                                .FirstOrDefault();

            if (reservacion == null)
            {
                return NotFound();
            }

            return Ok(reservacion);
        }

        /// <summary>
        /// Crea una nueva reservación.
        /// </summary>
        /// <param name="reservacion">Datos de la nueva reservación</param>
        /// <returns>Resultado de la operación de creación</returns>
        public IHttpActionResult Post(Reservacion reservacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si la cotización existe
                var cotizacion = db.Cotizaciones.Find(reservacion.IdCotizacion);
                if (cotizacion == null)
                {
                    return BadRequest("Cotización no encontrada.");
                }

                // Asignar la cotización
                reservacion.Cotizacion = cotizacion;

                // Calcular el saldo pendiente
                reservacion.CalcularSaldoPendiente();

                db.Reservas.Add(reservacion);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = reservacion.Id }, reservacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una reservación existente.
        /// </summary>
        /// <param name="id">ID de la reservación a actualizar</param>
        /// <param name="reservacion">Datos de la reservación actualizada</param>
        /// <returns>Resultado de la operación de actualización</returns>
        public IHttpActionResult Put(int id, Reservacion reservacion)
        {
            try
            {
                if (id != reservacion.Id)
                {
                    return BadRequest("ID no coincide.");
                }

                var existente = db.Reservas.Find(id);
                if (existente == null)
                {
                    return NotFound();
                }

                // Actualizar los campos de la reservación
                existente.FechaReservacion = reservacion.FechaReservacion;
                existente.Estado = reservacion.Estado;
                existente.FechaViaje = reservacion.FechaViaje;
                existente.FechaRegreso = reservacion.FechaRegreso;
                existente.MontoPagado = reservacion.MontoPagado;

                // Calcular el nuevo saldo pendiente
                existente.CalcularSaldoPendiente();

                db.Entry(existente).State = EntityState.Modified;
                db.SaveChanges();

                return Ok(existente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina una reservación por su ID.
        /// </summary>
        /// <param name="id">ID de la reservación a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            var reservacion = db.Reservas.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            db.Reservas.Remove(reservacion);
            db.SaveChanges();
            return Ok(reservacion);
        }
        */

    }

}
