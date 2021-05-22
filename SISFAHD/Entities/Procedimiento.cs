using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.DTOs;
using SISFAHD.Services;

namespace SISFAHD.Entities
{
    public class Procedimiento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("codigo_grupo")]
        public string codigo_grupo { get; set; }
        [BsonElement("nombre_grupo")]
        public string nombre_grupo { get; set; }
        [BsonElement("codigo_seccion")]
        public string codigo_seccion { get; set; }
        [BsonElement("seccion")]
        public string seccion { get; set; }
        [BsonElement("codigo_subseccion")]
        public string codigo_subseccion { get; set; }
        [BsonElement("subseccion")]
        public string subseccion { get; set; }
        [BsonElement("codigo_procedimiento")]
        public string codigo_procedimiento { get; set; }
        [BsonElement("procedimiento")]
        public string procedimiento { get; set; }
    }
}
