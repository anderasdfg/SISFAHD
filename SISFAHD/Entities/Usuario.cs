using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("usuario")]
        public string usuario { get; set; }

        [BsonElement("clave")]
        public string clave { get; set; }

    }
}
