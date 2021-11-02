using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class ResultadoExamenDTO
    {
        public ResultadoExamen resultadoExamen { get; set; } = new ResultadoExamen();
        public string idusuario { get; set; }
    }

    public class ResultadoExamenEliminarDTO
    {
        public string idResultado { get; set; }
        public string idUsuario { get; set; }
    }
}
