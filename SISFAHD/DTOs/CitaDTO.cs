using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;


namespace SISFAHD.DTOs
{
    public class CitaDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string estado_atencion { get; set; }
        public string estado_pago { get; set; }
        public DateTime? fecha_cita { get; set; }
        public DateTime? fecha_pago { get; set; }
        public string id_paciente { get; set; }
        public double precio_neto { get; set; }
        public string tipo_pago { get; set; }
        public ContenidoDatosUsuario datos_usuario { get; set; } = new ContenidoDatosUsuario();
        public ContenidoDatosPaciente datos_paciente { get; set; } = new ContenidoDatosPaciente();
        public ContenidoDatosTurno datos_turno { get; set; } = new ContenidoDatosTurno();
    }
    public class ContenidoDatosUsuario
    {
        public string id_usuario { get; set; }
    }
    public class ContenidoDatosPaciente
    {
        public DatosPaciente datos { get; set; } = new DatosPaciente();
        public string usuario { get; set; }
        public string clave { get; set; }
        public NombreRol nombre_rol { get; set; } = new NombreRol();
    }
    public class DatosPaciente
    {
        public string nombre_apellido_paciente { get; set; }
        public string correo { get; set; }
    }
    public class NombreRol
    {
        public string nombre { get; set; }
    }
    public class ContenidoDatosTurno
    {
        public EspecialidadMedicoCita especialidad { get; set; }
        public ContenidoDatosMedicoCita datos_medico { get; set; }
    }
    public class EspecialidadMedicoCita
    {
        public string nombre { get; set; }
        public string codigo { get; set; }
    }
    public class ContenidoDatosMedicoCita
    {
        public string nombre_apellido_medico { get; set; }
    }
}
