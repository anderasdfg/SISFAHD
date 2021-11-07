using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.DTOs
{
    public class PedidoDTO
    {
        public string _id { get; set; }
        public int cantidad { get; set; }
        public PacienteXPedido paciente { get; set; }
        public Datos datos_usua { get; set; }
        public string id_usuario { get; set; }
    }
    public class PacienteXPedido
    {
        public Datos_Paciente datos { get; set; }
        public string id_historia { get; set; }
        public string id_usuario { get; set; }
        public List<Archivos> archivos { get; set; }
    }
}
