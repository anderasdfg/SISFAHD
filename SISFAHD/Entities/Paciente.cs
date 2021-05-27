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
        public string idHistoria { get; set; }
        [BsonElement("id_usuario")]
        public string idUsuario { get; set; }
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
        public List<Personales> personales { get; set; }
        public List<Familiares> familiares { get; set; }
        public Psicosociales psicosociales { get; set; }
        public Sexuales sexuales { get; set; }
        public List<ProblemasCronicos> problemas_cronicos { get; set; }
    }

    public class ProblemasCronicos
    {
        public string numero { get; set; }
        public DateTime? fecha { get; set; }
        public string problema { get; set; }
        public string situacion { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Personales
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }
    public class Familiares
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }
    public class Psicosociales
    {
        public List<String> educacion { get; set; } = new List<String>();
        public List<String> laborales { get; set; } = new List<String>();
        public List<String> habitos_nocivos { get; set; } = new List<String>();
        public List<String> medicacion_habitual { get; set; } = new List<String>();
        public List<String> habitos_generales { get; set; } = new List<String>();
        public List<String> sociales { get; set; } = new List<String>();
    }
    public class Sexuales
    {
        public Espermarquia espermarquia { get; set; } 
        public Inicio_actividad_sexual inicio_actividad_sexual { get; set; }
        public Parejas_sexuales parejas_sexuales { get; set; }
        public Percepcion_libido percepcion_libido { get; set; }
        public Uso_metodos_anticonceptivos uso_metodos_anticonceptivos { get; set; }
    }

    public class Uso_metodos_anticonceptivos
    {
        public List<Metodos> metodos { get; set; }
        public bool estado { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Metodos
    {
        public string nombre { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Percepcion_libido
    {
        public string estado_percepcion { get; set; }
        public bool estado { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Parejas_sexuales
    {
        public int cantidad { get; set; }
        public bool parejas_simultaneas { get; set; }
        public bool estado { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Inicio_actividad_sexual
    {
        public int edad { get; set; }
        public bool estado { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }

    public class Espermarquia
    {
        public bool estado { get; set; }
        public List<String> observaciones { get; set; } = new List<String>();
    }
    public class Datos_Paciente
    {
        public string lugar_nacimiento { get; set; }
        public string procedencia { get; set; }
        public string grupo_instruccion { get; set; }
        public string estado_civil { get; set; }
        public string domicilio { get; set; }
        public string ocupacion { get; set; }
        public string grupo_sanguineo { get; set; }
        public List<Tutores> tutores_legales { get; set; } 
        
    }

    public class Tutores
    {
        public string parentesco { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
    }

}
