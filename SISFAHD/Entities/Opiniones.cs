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
    public class Opiniones
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("datos_medico")]
        public Dato_medico datos_medico { get; set; }

        [BsonElement("datos_paciente")]
        public Datos_paciente datos_paciente { get; set; }

        [BsonElement("calificacion")]
        public Double calificacion { get; set; }

        [BsonElement("observacion")]
        public string observacion { get; set; }

        [BsonElement("fecha_opinion")]
        public DateTime fecha_opinion { get; set; }

        [BsonElement("datos_cita")]
        public Datos_cita datos_cita { get; set; }


        public class Dato_medico { 
            public string id_medico { get; set; }
            public string nombre { get; set; }

        }
        public class Datos_paciente
        {
            public string id_paciente { get; set; }
            public string nombre { get; set; }
            public string apellido { get; set; }
        }
        public class Datos_cita
        {
            public DateTime fecha { get; set; }
            public string id_cita { get; set; }
        }
    }
}
