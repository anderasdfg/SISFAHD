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
    public class Adicionales
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("titulo")]
        public string titulo { get; set; }

        [BsonElement("descripcion")]
        public string descripcion {get; set;}

        [BsonElement("monto")]
        public double monto { get; set; }

        [BsonElement("url")]
        public string url { get; set; }
      
    }

}
