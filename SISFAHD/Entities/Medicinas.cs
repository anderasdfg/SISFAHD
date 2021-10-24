using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Entities
{
    public class Medicinas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("descripcion")]
        public string descripcion { get; set; }
        [BsonElement("generico")]
        public string generico { get; set; }
        [BsonElement("familia")]
        public string familia { get; set; }
        [BsonElement("precio")]
        public double precio { get; set; }
    }
}
