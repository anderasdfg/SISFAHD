using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;
namespace SISFAHD.DTOs
{
    public class PacienteDTOInfoMedica
    {
        public string id { get; set; }
        public Datos_Paciente datos { get; set; }
        public Antecedentes antecedentes { get; set; }
    }
}
