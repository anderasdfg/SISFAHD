using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using SISFAHD.Services;

namespace SISFAHD.DTOs
{
    public class EstadisticaDTO
    {
        public string _id { get; set; }
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
    public class CitasxMedicos
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public List<Turnos> turnos { get; set; }
        public Suscripcion suscripcion { get; set; }
        public Datos_basicos datos_basicos { get; set; }
        public string id_especialidad { get; set; }
        public List<Cita> citas { get; set; }
        public int cantidad { get; set; }
    }
    public class CitasxMedicosyEstadoAtencion
    {
        public int cantidad { get; set; }
        public Medico datos_medico { get; set; }
        public string estado_atencion { get; set; }
    }
    public class CitasxEspecialidad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int cantidad { get; set; }
        public Especialidad datos_especialidad { get; set; }
        public string nombre { get; set; }
    }
    public class CitasxEspecialidadyEstadoAtencion
    {
        public int cantidad { get; set; }
        public Especialidad datos_especialidad { get; set; }
        public string estado_atencion { get; set; }
    }
    public class CitasxEstadoAtencion
    {
        public int cantidad { get; set; }
        public string estado_atencion { get; set; }
    }
    public class CitasxPaciente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public Datos_Paciente datos { get; set; }
        public Usuario datos_paciente { get; set; }
        public List<Cita> datos_citas { get; set; }
        public int cantidad { get; set; }
    }
    public class CitasxPacienteyEstadoAtencion
    {
        public int cantidad { get; set; }
        public string estado_cita { get; set; }
        public Usuario datos_usuario { get; set; }      
        public string id_usuario { get; set; }
    }
    public class CitasDeMedicoXIdUsuario_y_EstadoPago
    {
        public int cantidad { get; set; }
        public string estado_atencion { get; set; }
        public string estado_pago { get; set; }
        public string id_usuario { get; set; }
        public string Nombre_medico { get; set; }
    }
}
