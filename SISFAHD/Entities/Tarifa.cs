using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Tarifa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("descripcion")]
        public string descripcion { get; set; }
        [BsonElement("impuesto")]
        public double impuesto { get; set; }
        [BsonElement("subtotal")]
        public double subtotal { get; set; }
        [BsonElement("precio_final")]
        public double precio_final { get; set; }
        [BsonElement("id_medico")]
        public string id_medico { get; set; }
    }
}
