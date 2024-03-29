﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Turno
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("especialidad")]
        public EspecialidadT especialidad { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }
        [BsonElement("fecha_fin")]
        public DateTime? fecha_fin { get; set; }
        [BsonElement("fecha_inicio")]
        public DateTime? fecha_inicio { get; set; }
        [BsonElement("hora_fin")]
        public string hora_fin { get; set; }
        [BsonElement("hora_inicio")]
        public string hora_inicio { get; set; }
        [BsonElement("id_medico")]
        public string id_medico { get; set; }
        [BsonElement("id_tarifa")]
        public string id_tarifa { get; set; }
        [BsonElement("Cupos")]
        public string Cupos { get; set; }



    }
    public class EspecialidadT
    {
        public string nombre { get; set; }
       
        public string codigo { get; set; }

    }
}
