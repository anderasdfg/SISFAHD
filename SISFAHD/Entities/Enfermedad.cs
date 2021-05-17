using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Enfermedad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("codigo_cie")]
        public string codigo_cie { get; set; }
        [BsonElement("descripcion")]
        public string descripcion { get; set; }
    }
}
}
