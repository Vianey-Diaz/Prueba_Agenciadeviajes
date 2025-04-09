using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Reservacion
    {
        [Key]
        public int Id { get; set; }

        public int IdCotizacion { get; set; }

        [ForeignKey("IdCotizacion")]
        public virtual Cotizacion Cotizacion { get; set; }

        public DateTime FechaReservacion { get; set; }
        public string Estado { get; set; } // Ejemplo: Confirmada, Cancelada, En Proceso
        public DateTime FechaViaje { get; set; }
        public DateTime FechaRegreso { get; set; }
        public double MontoPagado { get; set; }
        public double Saldopendiente { get; private set; }

        // Constructor vacío
        public Reservacion() { }

        // Método para calcular saldo pendiente
        public double CalcularSaldoPendiente()
        {
            if (Cotizacion == null)
            {
                throw new ArgumentNullException(nameof(Cotizacion), "La cotización no puede ser nula.");
            }

            if (Cotizacion.CostoTotal < 0)
            {
                throw new ArgumentException("El costo total no puede ser negativo.");
            }

            if (MontoPagado < 0)
            {
                throw new ArgumentException("El monto pagado no puede ser negativo.");
            }

            Saldopendiente = Cotizacion.CostoTotal - MontoPagado;
            return Saldopendiente;
        }

        public bool EsPagada()
        {
            return CalcularSaldoPendiente() == 0;
        }

    }
}

