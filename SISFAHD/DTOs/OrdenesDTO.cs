using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class OrdenesDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public DateTime fecha_orden { get; set; }
        public string id_usuario { get; set; }
        public MedicoOrden datos_medico { get; set; }
        public List<ExamenesOrden> examenes { get; set; }
    }
    public class MedicoOrden
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string especialidad { get; set; }
    }
    public class ExamenesOrden
    {
        public string estado { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public List<string> observaciones { get; set; } = new List<string>();
        public string tipo { get; set; }
        public List<doc_Anexo> resultado { get; set; }
    }

    public class OrdenesDTO_GetAll
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string estado_atencion { get; set; }
        public string estado_pago { get; set; }
        public DateTime fecha_orden { get; set; }
        public DateTime? fecha_pago { get; set; }
        public DateTime? fecha_reserva { get; set; }
        public string id_paciente { get; set; }
        public Int32 precio_neto { get; set; }
        public string tipo_pago { get; set; }
        public string usuario { get; set; }
        public List<ProcedimientosDetallado> procedimientos { get; set; }
        public ActoMedico datos_acto_medico { get; set; }
        public Medico datos_medico_orden { get; set; }
    }
    public class ProcedimientosDetallado
    {

        public string id_examen { get; set; }
        public string estado { get; set; }
        public string id_turno_orden { get; set; }
        public string id_resultado_examen { get; set; }
        public Examenes datos_examen { get; set; }
    }
    public class DatosOrden
    {
        public string id_paciente { get; set; }
        public Int32 precio_neto { get; set; }
        public string id_medico { get; set; }
    }
}
