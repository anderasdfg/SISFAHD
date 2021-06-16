using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;
using MongoDB.Bson;
using SISFAHD.DTOs;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;
        private readonly IMongoCollection<Medico> _medico;
        public UsuarioService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _usuarios = database.GetCollection<Usuario>("usuarios");
            _medico = database.GetCollection<Medico>("medicos");
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();
            usuarios = _usuarios.Find(Usuario => true).ToList();
            return usuarios;
        }
        public Usuario GetByUserNameAndPass(string username, string pass)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarios.Find(usuario => usuario.usuario == username & usuario.clave == pass).FirstOrDefault();
            return usuario;
        }
        public Usuario GetById(string id)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarios.Find(usuario => usuario.id == id).FirstOrDefault();
            return usuario;
        }

        public Usuario CreateUsuario(Usuario usuario)
        {
            _usuarios.InsertOne(usuario);
            return usuario;
        }

        public Usuario CreateUsuarioMedico(UsuarioMedico usuario)
        {
            //Pasando los datos del usuariomedico a una clase usuario
            Usuario miusuario = new Usuario();
            miusuario.usuario = usuario.usuario;
            //Completar chicas, sino hay castigo
            //
            _usuarios.InsertOne(miusuario);
            //Ya se inserto el usuario
            Medico mimedico = new Medico();
            //FALTA EL ID DEL USUARIO
            mimedico.id_especialidad = usuario.id_especialidad;
            //Completar chicas, sino hay castigo
            //
            _medico.InsertOne(mimedico);

            return miusuario;
        }

        public Usuario ModificarUsuario(Usuario usuario)
        {

            var filter = Builders<Usuario>.Filter.Eq("id", usuario.id);
            var update = Builders<Usuario>.Update
                .Set("nombre", usuario.datos.nombre)
                .Set("apellido paterno", usuario.datos.apellido_Paterno)
                .Set("apellido materno", usuario.datos.apellido_Materno)
                .Set("tipo de documento", usuario.datos.tipo_Documento)
                .Set("numero de documento", usuario.datos.numero_Documento)
                .Set("fecha de nacimiento", usuario.datos.fecha_nacimiento)
                .Set("nro de telefono", usuario.datos.telefono)
                .Set("sexo", usuario.datos.sexo);

            _usuarios.UpdateOne(filter, update);

            return usuario;
        }

        public async Task<List<UsuarioDTO>> GetAllUsuarios()
        {
            List<UsuarioDTO> gusuario = new List<UsuarioDTO>();
           
            var addFields = new BsonDocument("$addFields",
     new BsonDocument("id_rol",
     new BsonDocument("$toObjectId", "$rol")));
            var lookup = new BsonDocument("$lookup",
             new BsonDocument
                 {
            { "from", "roles" },
            { "localField", "id_rol" },
            { "foreignField", "_id" },
            { "as", "urol" }
                 });
            var unwind = new BsonDocument("$unwind",
             new BsonDocument
                 {
            { "path", "$urol" },
            { "preserveNullAndEmptyArrays", true }
                 });
            var addFields2 = new BsonDocument("$addFields",
              new BsonDocument("datos",
              new BsonDocument("apellidos",
              new BsonDocument("$concat",
              new BsonArray
                              {
                        "$datos.apellido_paterno",
                        " ",
                        "$datos.apellido_materno"
                              }))));
            var project = new BsonDocument("$project",
             new BsonDocument
                 {
            { "_id", 1 },
            { "datos", 1 },
            { "estado", 1 },
            { "urol",
    new BsonDocument("nombre", 1) }
                 });
            var project2 = new BsonDocument("$project",
             new BsonDocument("datos",
             new BsonDocument
                     {
                { "apellido_paterno", 0 },
                { "apellido_materno", 0 }
                     }));
            var addFields3 = new BsonDocument("$addFields",
             new BsonDocument("datos",
             new BsonDocument("nombresyapellidos",
             new BsonDocument("$concat",
             new BsonArray
                             {
                        "$datos.nombre",
                        " ",
                        "$datos.apellidos"
                             }))));
            var project3 = new BsonDocument("$project",
            new BsonDocument("datos",
            new BsonDocument
                    {
                { "nombre", 0 },
                { "apellidos", 0 }
                    }));

            gusuario = await _usuarios.Aggregate()
                .AppendStage<dynamic>(addFields)
                .AppendStage<dynamic>(lookup)
                .AppendStage<dynamic>(unwind)
                .AppendStage<dynamic>(addFields2)
                .AppendStage<dynamic>(project)
                .AppendStage<dynamic>(project2)
                .AppendStage<dynamic>(addFields3)
                .AppendStage<UsuarioDTO>(project3)
                .ToListAsync();
            return gusuario;
        }
    }
}
