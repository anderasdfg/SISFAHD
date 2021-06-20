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

        public  async Task<Usuario> CreateUsuarioMedico(UsuarioMedico usuario)
        {
            //Pasando los datos del usuariomedico a una clase usuario
            Usuario miusuario = new Usuario();
            miusuario.usuario = usuario.usuario;
            miusuario.clave = usuario.clave;
            miusuario.datos = usuario.datos;
            miusuario.rol = usuario.rol;
            miusuario.estado = "activo";
            miusuario.fecha_creacion = new System.DateTime();
            
            await _usuarios.InsertOneAsync(miusuario);
            //Ya se inserto el usuario
            Medico mimedico = new Medico();
            //FALTA EL ID DEL USUARIO
            mimedico.id_especialidad = usuario.id_especialidad;
            mimedico.id_usuario = miusuario.id;
            mimedico.datos_basicos = usuario.datos_basicos;
          
            await _medico.InsertOneAsync(mimedico);

            return miusuario;
        }


        public async Task<Usuario>UpdateUsuarioMedico(UsuarioMedico usuario)
        {
            var filterId = Builders<Usuario>.Filter.Eq("id", usuario.id);
            var update = Builders<Usuario>.Update
               .Set("datos.nombre", usuario.datos.nombre)
                .Set("datos.apellido_paterno", usuario.datos.apellido_paterno)
                .Set("datos.apellido_materno", usuario.datos.apellido_materno)
                .Set("datos.tipo_documento", usuario.datos.tipo_documento)
                .Set("datos.numero_documento", usuario.datos.numero_documento)
                .Set("datos.telefono", usuario.datos.fecha_nacimiento)
                .Set("datos.fecha_nacimiento", usuario.datos.telefono)
                .Set("datos.correo", usuario.datos.correo)
                .Set("datos.sexo", usuario.datos.sexo)
                //.Set("datos.foto", usuario.datos.foto)
                .Set("usuario", usuario.usuario)
                .Set("clave", usuario.clave);

            var resultado = await _usuarios.FindOneAndUpdateAsync<Usuario>(filterId, update, new FindOneAndUpdateOptions<Usuario>
            {
                ReturnDocument = ReturnDocument.After
            });

            var filterIdM = Builders<Medico>.Filter.Eq("id_usuario", usuario.id_usuario);
             var updateM = Builders<Medico>.Update

            .Set("datos_basicos.lugar_trabajo", usuario.datos_basicos.lugar_trabajo)
            .Set("datos_basicos.numero_colegiatura", usuario.datos_basicos.numero_colegiatura)
            .Set("datos_basicos.idiomas", usuario.datos_basicos.idiomas)
            .Set("datos_basicos.universidad", usuario.datos_basicos.universidad)
            .Set("datos_basicos.experiencia", usuario.datos_basicos.experiencia)
            .Set("datos_basicos.cargos", usuario.datos_basicos.cargos);

            //_usuarios.UpdateOneAsync(filterId, update);

            var resultadoM = await _medico.FindOneAndUpdateAsync<Medico>(filterIdM, updateM, new FindOneAndUpdateOptions<Medico>
            {
                ReturnDocument = ReturnDocument.After
            });

            return resultado;
        }

        public Usuario ModificarUsuario(Usuario usuario)
        {

            var filter = Builders<Usuario>.Filter.Eq("id", usuario.id);
            var update = Builders<Usuario>.Update
                .Set("datos.nombre", usuario.datos.nombre)
                .Set("datos.apellido_paterno", usuario.datos.apellido_paterno)
                .Set("datos.apellido_materno", usuario.datos.apellido_materno)
                .Set("datos.tipo_documento", usuario.datos.tipo_documento)
                .Set("datos.numero_documento", usuario.datos.numero_documento)
                .Set("datos.telefono", usuario.datos.fecha_nacimiento)
                .Set("datos.fecha_nacimiento", usuario.datos.telefono)
                .Set("datos.correo", usuario.datos.correo)
                .Set("datos.sexo", usuario.datos.sexo)
                //.Set("datos.foto", usuario.datos.foto)
                .Set("usuario", usuario.usuario)
                .Set("clave", usuario.clave);

            _usuarios.UpdateOne(filter, update);

            return usuario;
        }

        public Usuario GetByID(string id)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarios.Find(miUsuario => miUsuario.id == id).FirstOrDefault();
            return usuario;

        }

        public async Task<UsuarioMedico> GetByIDmedico(string id)
        {
            var match1 = new BsonDocument("$match",
                         new BsonDocument("_id",
                         new ObjectId(id)));
            var lookup1 = new BsonDocument("$lookup",
    new BsonDocument
        {
            { "from", "medicos" },
            { "let",
    new BsonDocument("idusum", "$_id") },
            { "pipeline",
    new BsonArray
            {
                new BsonDocument("$match",
                new BsonDocument("$expr",
                new BsonDocument("$eq",
                new BsonArray
                            {
                                new BsonDocument("$toObjectId", "$id_usuario"),
                                "$$idusum"
                            })))
            } },
            { "as", "usuarioresultado" }
        });

            var proyect1 = new BsonDocument("$project",
    new BsonDocument
        {
            { "datos", 1 },
            { "usuario", 1 },
            { "clave", 1 },
            { "fecha_creacion", 1 },
            { "rol", 1 },
            { "estado", 1 },
            { "medicoObj",
    new BsonDocument("$arrayElemAt",
    new BsonArray
                {
                    "$usuarioresultado",
                    -1
                }) }
        });

            var proyect2 = new BsonDocument("$project",
    new BsonDocument
        {
            { "datos", 1 },
            { "usuario", 1 },
            { "clave", 1 },
            { "fecha_creacion", 1 },
            { "rol", 1 },
            { "estado", 1 },
            { "id_usuario", "$medicoObj.id_usuario" },
            { "datos_basicos", "$medicoObj.datos_basicos" },
            { "id_especialidad", "$medicoObj.id_especialidad" }
        });

            UsuarioMedico miUsuarioMedico = await _usuarios.Aggregate()
                .AppendStage<dynamic>(match1)
                .AppendStage<dynamic>(lookup1)
                .AppendStage<dynamic>(proyect1)
                .AppendStage<UsuarioMedico>(proyect2)
                .FirstOrDefaultAsync();

            return miUsuarioMedico;

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
