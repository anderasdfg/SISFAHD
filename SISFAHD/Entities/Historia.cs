using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Historia
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("historial")]
        public string historial { get; set; }
        [BsonElement("fecha_creacion")]
        public DateTime? fecha_creacion { get; set; }
        [BsonElement("numero_historia")]
        public string numero_historia{ get; set; }
    }
}
