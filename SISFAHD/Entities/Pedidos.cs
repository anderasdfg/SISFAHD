using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Entities
{
    public class Pedidos
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("paciente")]
        public PacientePedidos paciente { get; set; }
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("id_acto_medico")]
        public string id_acto_medico { get; set; }
        [BsonElement("productos")]
        public List<Productos> productos { get; set; }
        [BsonElement("estado_pago")]
        public string estado_pago { get; set; }
        [BsonElement("fecha_creacion")]
        public DateTime fecha_creacion { get; set; }
        [BsonElement("fecha_pago")]
        public DateTime fecha_pago { get; set; }
        [BsonElement("precio_neto")]
        public int precio_neto { get; set; }
    }
    public class PacientePedidos
    {
        public string id_paciente { get; set; }
        public string nombre { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
    }
    public class Productos
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int precio { get; set; }
        public int cantidad { get; set; }
    }
}
