using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class TurnoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public EspecialidadT especialidad { get; set; }
        public string estado { get; set; }
        public DateTime fecha_fin { get; set; }
        public DateTime fecha_inicio { get; set; }
        public string hora_fin { get; set; }
        public string hora_inicio { get; set; }
        public string id_medico { get; set; }
        public string id_tarifa { get; set; }
        public List<Cupos> cupos { get; set;  }
        public string nombre_medico { get; set; }
        public double precio{ get; set; }
    }

        
}
