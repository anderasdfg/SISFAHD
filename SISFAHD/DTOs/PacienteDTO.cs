using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class PacienteDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public diagnostico diagnostico { get; set; } = new diagnostico();
    }
    public class diagnostico
    {
        public examenes_auxiliares examenes_auxiliares { get; set; } = new examenes_auxiliares();
    }
    public class examenes_auxiliares
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public List<string> observaciones { get; set; }
        public string tipo { get; set; }
    }
}
