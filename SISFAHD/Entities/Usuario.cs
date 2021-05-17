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

        [BsonElement("fecha_creacion")]
        public DateTime fechaCreacion { get; set; }

        [BsonElement("rol")]
        public string rol { get; set; }
        [BsonElement("estado")]
        public string estado{ get; set; }

    }

    public class Datos
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string tipo_documento { get; set; }
        public string telefono { get; set; }
        public string numero_documento { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public string correo { get; set; }
        public string sexo { get; set; }

        [BsonElement("foto")]
        public string foto { get; set; }
    }
}
