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
    public class Paciente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("datos")]
        public Datos_Paciente datos { get; set; }
        [BsonElement("antecedentes")]
        public Antecedentes antecedentes { get; set; }
        [BsonElement("id_historia")]
        public string id_historia { get; set; }
        [BsonElement("id_usuario")]
        public string id_usuario { get; set; }
        [BsonElement("archivos")]
        public List<Archivos> archivos { get; set; }        
    }

    public class Archivos
    {
        public string descripcion { get; set; }
        public string url { get; set; }
    }

    public class Antecedentes
    {
        public Personales personales { get; set; }
        public Familiares familiares { get; set; }
        public Habitos habitos { get; set; }
        public Sexuales sexuales { get; set; }
    }

    public class Personales
    {
        public bool? existencia { get; set; }
        public List<Enfermedad_ant> enfermedades { get; set; } = new List<Enfermedad_ant>();
    }
    public class Familiares
    {
        public bool? existencia { get; set; }
        public List<Enfermedad_ant> enfermedades { get; set; } = new List<Enfermedad_ant>();
    }
    public class Enfermedad_ant
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string situacion { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public List<Observacion> observaciones { get; set; } = new List<Observacion>();
    }


    public class Habitos
    {
        public ConsumoSustancia consumo_tabaco { get; set; }
        public ConsumoSustancia consumo_alcohol { get; set; }
        public ConsumoSustancia consumo_drogas { get; set; }
    }

    public class ConsumoSustancia
    {
        public string consumo { get; set; }
        public List<Observacion> observaciones { get; set; } = new List<Observacion>();
    }
    public class Sexuales
    {
        public Inicio_actividad_sexual inicio_actividad_sexual { get; set; }
        public string parejas_sexuales { get; set; }
        public Uso_metodos_anticonceptivos uso_metodos_anticonceptivos { get; set; }
    }

    public class Uso_metodos_anticonceptivos
    {
        public List<Metodos> metodos { get; set; }
        public bool? uso_metodos { get; set; }
    }

    public class Metodos
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public List<Observacion> observaciones { get; set; } = new List<Observacion>();
    }

    public class Inicio_actividad_sexual
    {
        public string edad { get; set; }
        public bool? estado { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Datos_Paciente
    {
        public string lugar_nacimiento { get; set; }
        public string estado_civil { get; set; }
        public string domicilio { get; set; }
        public string ocupacion { get; set; }
        public string grupo_sanguineo { get; set; }
        
    }

    public class Observacion
    {
        public string observacion { get; set; }
        public DateTime? fecha_observacion { get; set; }
    }

}
