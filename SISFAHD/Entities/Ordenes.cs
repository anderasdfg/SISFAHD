using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Entities
{
    public class Ordenes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("estado_atencion")]
        public string estado_atencion { get; set; }

        [BsonElement("fecha_orden")]
        public DateTime fecha_orden { get; set; }

        [BsonElement("id_paciente")]
        public string id_paciente { get; set; }


        [BsonElement("id_acto_medico")]
        public string id_acto_medico { get; set; }
        [BsonElement("id_medico_orden")]
        public string id_medico_orden { get; set; }
        [BsonElement("procedimientos")]
        public List<Procedimientos> procedimientos { get; set; }
    }
    public class Procedimientos
    {
        public string id_examen { get; set; }
        public string estado_resultado { get; set; }
        public string estado { get; set; }
        public DateTime? fecha_pago { get; set; }
        public DateTime? fecha_reserva { get; set; }
        public Double precio_neto { get; set; }
        public string tipo_pago { get; set; }
        public string id_turno_orden { get; set; }
        public string id_resultado_examen { get; set; }
    }
}
