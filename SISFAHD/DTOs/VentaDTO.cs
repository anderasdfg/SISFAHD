using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class VentaDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string codigo_orden { get; set; }
        public string estado { get; set; }
        public string detalle_estado { get; set; }
        public string tipo_operacion { get; set; }
        public string tipo_pago { get; set; }
        public string monto { get; set; }
        public string titular { get; set; }
        public string fecha_pago { get; set; }
        public string moneda { get; set; }
        public string codigo_referencia { get; set; }
        public ContenidoDatosCita datos_cita { get; set; } = new ContenidoDatosCita();
        public ContenidoDatosPaciente datos_paciente { get; set; } = new ContenidoDatosPaciente();
        public ContenidoDatosTurno datos_turno { get; set; } = new ContenidoDatosTurno();
    }
    public class ContenidoDatosCita
    {
        public string estado_atencion { get; set; }
        public string estado_pago { get; set; }
        public DateTime? fecha_cita { get; set; }
        public DateTime? fecha_pago { get; set; }
        public string id_paciente { get; set; }
        public double precio_neto { get; set; }
        public string tipo_pago { get; set; }
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
        public string hora_inicio { get; set; }
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
