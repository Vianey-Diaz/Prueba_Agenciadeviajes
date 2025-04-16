using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgenciadeViajesApi.Models
{
    public class Vuelo
    {
        private int _idVuelo;
        private string _tipo;
        private Destino _origen;
        private Destino _destino;
        private string _compañia;
        private TimeSpan _horaSalida;
        private TimeSpan _horaLlegada;
        private int _capacidad;
        private double _precio;

        // Claves foráneas
        public int OrigenId { get; set; }
        public int DestinoId { get; set; }

        // Propiedades públicas
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
        public string Compañia
        {
            get { return _compañia; }
            set { _compañia = value; }
        }
        public TimeSpan HoraSalida
        {
            get { return _horaSalida; }
            set { _horaSalida = value; }
        }
        public TimeSpan HoraLlegada
        {
            get { return _horaLlegada; }
            set { _horaLlegada = value; }
        }
        public int Capacidad
        {
            get { return _capacidad; }
            set { _capacidad = value; }
        }
        public double Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        // Propiedades de navegación
        public virtual Destino Origen { get; set; }
        public virtual Destino Destino { get; set; }


    }
}