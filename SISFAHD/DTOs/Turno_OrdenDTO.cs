﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class Turno_OrdenDTO_By_Especialidad_Fecha
    {
        public string idespecialidad { get; set; }
        public int año { get; set; }
        public int mes { get; set; }
        public int dia { get; set; }
    }
}
