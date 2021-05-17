using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SISFAHD.Entities
{
    public class Venta
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("codigo_orden")]
        public string codigo_orden { get; set; }
        [BsonElement("estado")]
        public string estado { get; set; }
        [BsonElement("detalle_estado")]
        public string detalle_estado { get; set; }
        [BsonElement("tipo_operacion")]
        public string tipo_operacion { get; set; }
        [BsonElement("tipo_pago")]
        public string tipo_pago { get; set; }
        [BsonElement("monto")]
        public string monto { get; set; }
        [BsonElement("titular")]
        public string titular { get; set; }
        [BsonElement("fecha_pago")]
        public string fecha_pago { get; set; }
        [BsonElement("moneda")]
        public string moneda { get; set; }
        [BsonElement("codigo_referencia")]
        public string codigo_referencia { get; set; }
    }

}
