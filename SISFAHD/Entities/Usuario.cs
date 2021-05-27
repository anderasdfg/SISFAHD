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
        [BsonElement("estado")]
        public string estado { get; set; }

    }

    public class Datos
    {
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("apellido_paterno")]
        public string apellidoPaterno { get; set; }
        [BsonElement("apellido_materno")]
        public string apellidoMaterno { get; set; }
        [BsonElement("tipo_documento")]
        public string tipoDocumento { get; set; }
        [BsonElement("telefono")]
        public string telefono { get; set; }
        [BsonElement("numero_documento")]
        public string numeroDocumento { get; set; }
        [BsonElement("fecha_nacimiento")]
        public DateTime fechanacimiento { get; set; }
        [BsonElement("correo")]
        public string correo { get; set; }
        [BsonElement("sexo")]
        public string sexo { get; set; }
        [BsonElement("foto")]
        public string foto { get; set; }
    }
}
