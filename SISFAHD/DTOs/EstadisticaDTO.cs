using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.DTOs
{
    public class EstadisticaDTO
    {
        public int cantidad { get; set; }
    }
    public class EspecialidadesMPedidas
    {
        public string _id { get; set;}
        public Int32 cantidad { get; set; }
        public DatosEspecMPedidas datos { get; set; }
    }
    public class DatosEspecMPedidas
    {
        public string nombre { get; set; }
        public string url { get; set; }
    }
    public class MedicamentosMPedidos
    {
        public string _id { get; set; }
        public Int32 cantidad { get; set; }
        public DatosMediMPedidos datos { get; set; }
    }
    public class DatosMediMPedidos
    {
        public string nombre { get; set; }
        public string concentracion { get; set; }
        public string formula_farmaceutica { get; set; }
    }
    public class LaboratorioPedidos
    {
        public string _id { get; set; }
        public Int32 cantidad { get; set; }
    }
}
