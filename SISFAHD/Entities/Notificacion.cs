﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Notificacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("titulo")]
        public string titulo { get; set; }
        [BsonElement("cuerpo")]
        public string cuerpo { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }
        [BsonElement("fecha_emision")]
        public DateTime? fecha_emision { get; set; }
        [BsonElement("id_emisor")]
        public string id_emisor { get; set; }
        [BsonElement("id_receptor")]
        public string id_receptor { get; set; }
    }
}
