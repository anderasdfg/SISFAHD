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
    public class ResultadoExamen
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }

        [BsonElement("observaciones")]
        public string observaciones { get; set; }

        [BsonElement("documento_anexo")]
        public List<doc_Anexo> documento_anexo { get; set; }

        [BsonElement("codigo")]
        public string codigo { get; set; }
    }
    public class doc_Anexo
    {
        public string titulo { get; set; }
        public string url { get; set; }
    }
}
