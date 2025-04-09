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
    public class FacturasController : ApiController
    {
        private Proyectodb db = new Proyectodb();

        /// <summary>
        /// Obtiene la lista de todas las facturas.
        /// </summary>
        /// <returns>Lista de facturas</returns>
        public IHttpActionResult Get()
        {
            var facturas = from F in db.Factura
                           select new
                           {
                               F.Id,
                               ReservacionId = F.Reservacion.Id,
                               FechaPago = F.FechaPago,
                               MontoPagado = F.MontoPagado,
                               MetodoPagoId = F.MetodoPago.Id,
                               Estado = F.Estado
                           };

            if (!facturas.Any())
            {
                return NotFound();
            }

            return Ok(facturas);
        }

        /// <summary>
        /// Obtiene una factura por su ID.
        /// </summary>
        /// <param name="id">ID de la factura</param>
        /// <returns>Factura correspondiente al ID</returns>
        public IHttpActionResult Get(int id)
        {
            var factura = db.Factura
                            .Where(F => F.Id == id)
                            .Select(F => new
                            {
                                F.Id,
                                ReservacionId = F.Reservacion.Id,
                                FechaPago = F.FechaPago,
                                MontoPagado = F.MontoPagado,
                                MetodoPagoId = F.MetodoPago.Id,
                                Estado = F.Estado
                            })
                            .FirstOrDefault();

            if (factura == null)
            {
                return NotFound();
            }

            return Ok(factura);
        }

        /// <summary>
        /// Crea una nueva factura.
        /// </summary>
        /// <param name="factura">Datos de la nueva factura</param>
        /// <returns>Resultado de la operación de creación</returns>
        public IHttpActionResult Post(Factura factura)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si la reservación existe
                var reservacion = db.Reservas.Find(factura.ReservacionId);
                if (reservacion == null)
                {
                    return BadRequest("Reservación no encontrada.");
                }

                // Verificar si el método de pago existe
                var metodoPago = db.MetododePagos.Find(factura.MetodoPagoId);
                if (metodoPago == null)
                {
                    return BadRequest("Método de pago no encontrado.");
                }

                // Asignar la reservación y el método de pago
                factura.Reservacion = reservacion;
                factura.MetodoPago = metodoPago;

                db.Factura.Add(factura);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = factura.Id }, factura);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una factura existente.
        /// </summary>
        /// <param name="id">ID de la factura a actualizar</param>
        /// <param name="factura">Datos de la factura actualizada</param>
        /// <returns>Resultado de la operación de actualización</returns>
        public IHttpActionResult Put(int id, Factura factura)
        {
            try
            {
                if (id != factura.Id)
                {
                    return BadRequest("ID no coincide.");
                }

                var existente = db.Factura.Find(id);
                if (existente == null)
                {
                    return NotFound();
                }

                // Actualizar los campos de la factura
                existente.FechaPago = factura.FechaPago;
                existente.MontoPagado = factura.MontoPagado;
                existente.Estado = factura.Estado;

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
        /// Elimina una factura por su ID.
        /// </summary>
        /// <param name="id">ID de la factura a eliminar</param>
        /// <returns>Resultado de la operación de eliminación</returns>
        public IHttpActionResult Delete(int id)
        {
            var factura = db.Factura.Find(id);
            if (factura == null)
            {
                return NotFound();
            }

            db.Factura.Remove(factura);
            db.SaveChanges();
            return Ok(factura);
        }
    }

}
