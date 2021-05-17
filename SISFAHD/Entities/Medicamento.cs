using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Medicamento
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("codigo")]
        public string codigo { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("concentracion")]
        public string concentracion { get; set; }
        [BsonElement("formula_farmaceutica")]
        public string formula_farmaceutica { get; set; }
        [BsonElement("formula_farmaceutica_simplificada")]
        public string formula_farmaceutica_simplificada { get; set; }
        [BsonElement("presentacion")]
        public string presentacion { get; set; }
        [BsonElement("fraccion")]
        public string fraccion { get; set; }
        [BsonElement("registro_sanitario")]
        public string registro_sanitario { get; set; }
        [BsonElement("laboratorio")]
        public string laboratorio { get; set; }
        [BsonElement("situacion")]
        public string situacion { get; set; }
    }
}
