using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Proyectodb : DbContext
    {
        public Proyectodb() : base("MyDbConnectionString")
        {
        }

        public DbSet<Factura> Factura { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Actividades> Actividades { get; set; }
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Metodo_Pago> MetododePagos { get; set; }
        public DbSet<Paquete_Turistico> PaqueteTuristicos { get; set; }
        public DbSet<Reservacion> Reservas { get; set; }
        public DbSet<Seguro> Seguros { get; set; }
        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<AgentedeViaje> AgenteViajes { get; set; }
        public DbSet<GuiaTuristico> GuiaTuristicos { get; set; }

        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<TipoHabitacion> TiposdeHabitacion {  get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre Vuelo y Destino (Origen)
            modelBuilder.Entity<Vuelo>()
                .HasRequired(v => v.Origen)
                .WithMany() // Relación de uno a muchos (suponiendo que Destino puede tener múltiples vuelos como origen)
                .HasForeignKey(v => v.OrigenId)
                .WillCascadeOnDelete(false); // Desactivar Cascade Delete

            // Relación entre Vuelo y Destino (Destino)
            modelBuilder.Entity<Vuelo>()
                .HasRequired(v => v.Destino)
                .WithMany() // Relación de uno a muchos (suponiendo que Destino puede tener múltiples vuelos)
                .HasForeignKey(v => v.DestinoId)
                .WillCascadeOnDelete(false); // Desactivar Cascade Delete
        }




    }
}