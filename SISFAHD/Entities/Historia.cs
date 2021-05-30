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
    public class Historia
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("historial")]
        public List<Historial> historial { get; set; }
        [BsonElement("fecha_creacion")]
        public DateTime? fecha_creacion { get; set; }
        [BsonElement("numero_historia")]
        public string numero_historia { get; set; }
    }

    public class Historial
    {
        public DateTime? fecha_cita { get; set; }
        public DateTime? hora_inicio { get; set; }
        public DateTime? hora_fin { get; set; }
        public Datos_medico datos_medico { get; set; }
        public string id_cita { get; set; }
    }

    public  class Datos_medico
    {
        public string nombre_medico { get; set; }
        public string id_medico { get; set; }
        public string nombre_especialidad { get; set; }
    }
}
