using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

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

        [BsonElement("datos")]
        public Datos datos { get; set; }

        [BsonElement("fechacreacion")]
        public DateTime fechaCreacion { get; set; }

        [BsonElement("rol")]
        public string rol { get; set; }

    }

    public class Datos
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string tipodocumento { get; set; }
        public string telefono { get; set; }
        public string numerodocumento { get; set; }
        public DateTime fechanacimiento { get; set; }
        public string correo { get; set; }
        public string sexo { get; set; }
    }
}
