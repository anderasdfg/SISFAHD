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
        public DateTime? fecha_creacion { get; set; }

        [BsonElement("rol")]
        public string rol { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }

    }

    public class UsuarioMedico
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
        public DateTime? fecha_creacion { get; set; }

        [BsonElement("rol")]
        public string rol { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }

        [BsonElement("datos_basicos")]
        public Datos_basicos datos_basicos { get; set; }
        [BsonElement("id_especialidad")]
        public string id_especialidad { get; set; }
        [BsonElement("id_usuario")]
        public string id_usuario { get; set; }

    }

    public class Datos
    {
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("apellido_paterno")]
        public string apellido_paterno { get; set; }
        [BsonElement("apellido_materno")]
        public string apellido_materno { get; set; }
        [BsonElement("tipo_documento")]
        public string tipo_documento { get; set; }
        [BsonElement("telefono")]
        public string telefono { get; set; }
        [BsonElement("numero_documento")]
        public string numero_documento { get; set; }
        [BsonElement("fecha_nacimiento")]
        public DateTime fecha_nacimiento { get; set; }
        [BsonElement("correo")]
        public string correo { get; set; }
        [BsonElement("sexo")]
        public string sexo { get; set; }
        [BsonElement("foto")]
        public string foto { get; set; }

        [BsonElement("codigo")]
        public string codigo { get; set; }
        

    }
}
