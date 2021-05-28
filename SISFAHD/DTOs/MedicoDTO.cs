using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class MedicoDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }        
        public List<Turnos> turnos { get; set; }        
        public Suscripcion suscripcion { get; set; }       
        public Datos_basicos datos_basicos { get; set; }        
        public string id_especialidad { get; set; }        
        public string id_usuario { get; set; }
        public string nombrecompleto { get; set; }

    }
}
