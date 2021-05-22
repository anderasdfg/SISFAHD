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
    public class Medico
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("turnos")]
        public Turnos turnos { get; set; }
        [BsonElement("suscripcion")]
        public Suscripcion suscripcion { get; set; }
        [BsonElement("datos_basicos")]
        public Datos_basicos datos_basicos { get; set; }
        [BsonElement("id_especialidad")]
        public string id_especialidad { get; set; }
        [BsonElement("id_usuario")]
        public string id_usuario { get; set; }
    }
    public class Turnos
    {
        public string estado { get; set; }
        public string codigo { get; set; }

    }
    public class Suscripcion
    {
        [BsonElement("fecha_inicio")]
        public DateTime? fecha_inicio { get; set; }
        [BsonElement("fecha_fin")]
        public DateTime? fecha_fin { get; set; }

    }
    public class Datos_basicos
    {
        public string lugar_trabajo { get; set; }
        public string numero_colegiatura { get; set; }
        public string idiomas { get; set; }
        public string universidad { get; set; }
        public string experiencia { get; set; }
        public string cargos { get; set; }
    }

}
