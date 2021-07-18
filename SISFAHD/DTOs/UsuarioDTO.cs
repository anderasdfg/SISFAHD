using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class UsuarioDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public DatosUsuario datos { get; set; } = new DatosUsuario();
        public string estado { get; set; }
        public userRol urol { get; set; }
    }
    public class userRol
    {
        public string nombre { get; set; }
    }
    public class DatosUsuario
    {
       
        public string nombre { get; set; }
        
        public string tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public string telefono { get; set; }
        
        
        public DateTime fecha_nacimiento { get; set; }
        
        public string correo { get; set; }
       
        public string sexo { get; set; }
       
        public string foto { get; set; }
        public string nombresyapellidos { get; set; }

        public string codigo { get; set; }
    }
}
