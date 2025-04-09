using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Range(1, 5)]
        public int Estrellas { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Direccion { get; set; }

        // Aquí, asumimos que la clase TipoHabitacion es otra entidad con una clave primaria
        [Required]
        public int TipoHabitacionId { get; set; }

        [ForeignKey("TipoHabitacionId")]
        public virtual TipoHabitacion Tipohabitacion { get; set; }  // Relación con TipoHabitacion

        // Constructor vacío
        public Hotel() { }

        // Constructor con parámetros
        public Hotel(int id, string nombre, int estrellas, string direccion, TipoHabitacion tipohabitacion)
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre), "El nombre no puede ser nulo");
            Estrellas = estrellas >= 1 && estrellas <= 5 ? estrellas : throw new ArgumentException("Las estrellas deben estar entre 1 y 5");
            Direccion = direccion ?? throw new ArgumentNullException(nameof(direccion), "La dirección no puede ser nula");
            Tipohabitacion = tipohabitacion ?? throw new ArgumentNullException(nameof(tipohabitacion), "El tipo de habitación no puede ser nulo");
        }

    }
}