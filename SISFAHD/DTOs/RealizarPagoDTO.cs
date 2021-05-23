using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;


namespace SISFAHD.DTOs
{
    public class RealizarPagoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string estado_atencion { get; set; }
        public string estado_pago { get; set; }
        public DateTime? fecha_cita { get; set; }

        public DateTime? fecha_pago { get; set; }
        public string id_paciente { get; set; }
        public Double precio_neto { get; set; }
        public string tipo_pago { get; set; }
        public ContenidoDatosUsuario datos_usuario { get; set; }
        public ContenidoDatosPaciente datos_paciente { get; set; }
    }
    public class ContenidoDatosUsuario
    {
        public string id_usuario { get; set; }
    }
    public class ContenidoDatosPaciente
    {
        public DatosPaciente datos { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public NombreRol nombre_rol { get; set; }
    }
    public class DatosPaciente
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
    }
    public class NombreRol
    {
        public string nombre { get; set; }
    }
}
