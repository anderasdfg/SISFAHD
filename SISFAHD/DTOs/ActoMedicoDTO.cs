using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.DTOs
{
    public class ActoMedicoDTO
    {
        public Medicacion medicacion { get; set; }
        public List<Diagnostico> diagnostico { get; set; } = new List<Diagnostico>();
        public SignosVitales signos_vitales { get; set; }
        public DateTime? fecha_atencion { get; set; }
        public string anamnesis { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public string indicaciones { get; set; }
    }
}
