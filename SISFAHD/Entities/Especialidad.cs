using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Especialidad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("codigo")]
        public string codigo { get; set; }
        [BsonElement("descripcion")]
        public string descripcion { get; set; }


    }
}
