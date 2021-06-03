using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

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
        public ContenidoDatosPaciente datos_paciente { get; set; } = new ContenidoDatosPaciente();
        public ContenidoDatosTurno datos_turno { get; set; } = new ContenidoDatosTurno();
    }
    
    // CITADTO -> Correctamente Mapeado
    public class CitaDTO2
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string estado_atencion { get; set; }
        public string estado_pago { get; set; }
        public DateTime? fecha_cita { get; set; }
        public DateTime? fecha_pago { get; set; }
        public DateTime? fecha_reserva { get; set; }
        public Paciente datos_paciente { get; set; }
        public string enlace_cita { get; set; }
        public double precio_neto { get; set; }
        public double calificacion { get; set; }
        public List<String> observaciones { get; set; } = new List<string>();
        public string tipo_pago { get; set; }
        public string id_turno { get; set; }
        public Turno turno { get; set; }
        public ActoMedico datos_acto_medico { get; set; }
        public DateTime? fecha_cita_fin { get; set; }
    }
}
