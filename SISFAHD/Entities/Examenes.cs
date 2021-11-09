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
    public class Examenes
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("descripcion")]
        public string descripcion { get; set; }
        [BsonElement("precio")]
        public double precio { get; set; }
        [BsonElement("id_especialidad")]
        public string idEspecialidad { get; set; }
        [BsonElement("duracion")]
        public string duracion { get; set; }
        [BsonElement("recomendaciones_previas")]
        public string recomendacionesPrevias { get; set; }
        [BsonElement("recomendaciones_posteriores")]
        public string recomendacionesPosteriores { get; set; }


        [BsonElement("id_especialidad")]
        public string id_especialidad { get; set; }

        [BsonElement("duracion")]
        public string duracion { get; set; }

        [BsonElement("recomendaciones_previas")]
        public string recomendaciones_previas { get; set; }

        [BsonElement("recomendaciones_posteriores")]
        public string recomendaciones_posteriores { get; set; }


    }
}
