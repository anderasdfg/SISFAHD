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
        public Datos datos { get; set; }
        public string estado { get; set; }
        public userRol urol { get; set; }
    }
    public class userRol
    {
        public string nombre { get; set; }
    }
}
