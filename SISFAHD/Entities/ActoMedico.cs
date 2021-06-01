using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Entities
{
    public class ActoMedico
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("medicacion")]
        public Medicacion medicacion { get; set; }
        [BsonElement("diagnostico")]
        public List<Diagnostico> diagnostico { get; set; } = new List<Diagnostico>();
        [BsonElement("signos_vitales")] 
        public SignosVitales signosVitales { get; set; }
        [BsonElement("fecha_atencion")]
        public DateTime? fechaAtencion { get; set; }
        [BsonElement("anamnesis")]
        public string anamnesis { get; set; }
        [BsonElement("fecha_creacion")]
        public DateTime? fechaCreacion { get; set; }
        [BsonElement("indicaciones")]
        public string indicaciones { get; set; }
    }


    #region Medicacion
    public class Medicacion {
        [BsonElement("medicacion_previa")]
        public List<MedicacionPrevia> medicacionesPrevias { get; set; } = new List<MedicacionPrevia>();
        [BsonElement("reaccion_adversa")]
        public List<string> reaccionAdversa { get; set; }
    }

    public class MedicacionPrevia {
        [BsonElement("codigo")]
        public string codigo { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("dosis")]
        public string dosis { get; set; }
        [BsonElement("observaciones")]
        public List<string> observaciones { get; set; } = new List<string>();
    }
    #endregion
    #region Diagnostico
    public class Diagnostico {
        [BsonElement("codigo_enfermedad")]
        public string codigoEnfermedad { get; set; }
        [BsonElement("nombre_enfermedad")]
        public string nombreEnfermedad { get; set; }
        [BsonElement("observaciones")]
        public List<string> observaciones { get; set; } = new List<string>();
        [BsonElement("tipo")]
        public string tipo { get; set; }
        [BsonElement("frecuencia")]
        public string frecuencia { get; set; }
        [BsonElement("examenes_auxiliares")]
        public List<ExamenAuxiliar> examenesAuxiliares { get; set; } = new List<ExamenAuxiliar>();
        [BsonElement("prescripcion")]
        public List<Prescripcion> prescripcion { get; set; } = new List<Prescripcion>();
    }

    public class ExamenAuxiliar {
        [BsonElement("codigo")]
        public string codigo { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("observaciones")]
        public List<string> observaciones { get; set; } = new List<string>();
        [BsonElement("tipo")]
        public string tipo { get; set; }
    }

    public class Prescripcion {
        [BsonElement("codigo")]
        public string codigo { get; set; }
        [BsonElement("nombre")]
        public string nombre { get; set; }
        [BsonElement("formula")]
        public string formula { get; set; }
        [BsonElement("concentracion")]
        public string concentracion { get; set; }
        [BsonElement("dosis")]
        public Dosis dosis { get; set; }
    }

    public class Dosis {
        [BsonElement("frecuencia")]
        public Administracion frecuencia { get; set; }
        [BsonElement("tiempo")]
        public Administracion tiempo { get; set; }
        [BsonElement("cantidad")]
        public string cantidad { get; set; }
        [BsonElement("via_administracion")]
        public string viaAdministracion { get; set; }
        [BsonElement("observaciones")]
        public List<string> observaciones { get; set; } = new List<string>();

    }

    public class Administracion { 
        public string valor { get; set; }
        public string medida { get; set; }
    }
    #endregion
    #region SignosVitales
    public class SignosVitales {
        [BsonElement("constantes_vitales")]
        public ConstanteVital constantesVitales { get; set; }
        [BsonElement("datos_antropometricos")]
        public DatosAntropomorficos datosAntropomorficos { get; set; }
    }

    public class ConstanteVital
    {
        [BsonElement("temperatura")]
        public Administracion temperatura { get; set; }
        [BsonElement("presion_arterial")]
        public Administracion presionArterial { get; set; }
        [BsonElement("saturacion")]
        public Administracion saturacion { get; set; }
        [BsonElement("frecuencia_cardiaca")]
        public Administracion frecuenciaCardiaca { get; set; }
        [BsonElement("frecuencia_respiratoria")]
        public Administracion frecuenciaRespiratoria { get; set; }
    }
    public class DatosAntropomorficos
    {
        [BsonElement("peso")]
        public Administracion peso { get; set; }
        [BsonElement("talla")]
        public Administracion talla { get; set; }
        [BsonElement("perimetro_abdominal")]
        public Administracion perimetroAbdominal { get; set; }
        [BsonElement("superficie_corporal")]
        public Administracion superficieCorporal { get; set; }
        [BsonElement("imc")]
        public Double imc { get; set; } 
        [BsonElement("clasificacion_imc")]
        public string clasificacion_imc { get; set; }
    }
    #endregion
}
