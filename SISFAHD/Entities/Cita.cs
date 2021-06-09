using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.DTOs;
using SISFAHD.Services;

namespace SISFAHD.Entities
{
    public class Cita
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("estado_atencion")]
        public string estado_atencion { get; set; }
        [BsonElement("estado_pago")]
        public string estado_pago { get; set; }
        [BsonElement("fecha_cita")]
        public DateTime? fecha_cita { get; set; }
        [BsonElement("fecha_pago")]
        public DateTime? fecha_pago { get; set; }
        [BsonElement("fecha_reserva")]
        public DateTime? fecha_reserva { get; set; }
        [BsonElement("id_paciente")]
        public string id_paciente { get; set; }
        [BsonElement("enlace_cita")]
        public string enlace_cita { get; set; }
        [BsonElement("precio_neto")]
        public double precio_neto { get; set; }
        [BsonElement("calificacion")]
        public double calificacion { get; set; }
        [BsonElement("observaciones")]
        public List<String> observaciones { get; set; } = new List<string>();
        [BsonElement("tipo_pago")]
        public string tipo_pago { get; set; }
        [BsonElement("id_turno")]
        public string id_turno { get; set; }
        [BsonElement("id_acto_medico")]
        public string id_acto_medico { get; set; }
        [BsonElement("fecha_cita_fin")]
        public DateTime? fecha_cita_fin { get; set; }
        [BsonElement("motivo_consulta")]
        public string motivo_consulta { get; set; }
        [BsonElement("id_medico")]
        public string id_medico { get; set; }

    }
}
