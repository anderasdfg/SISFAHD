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
        public UsuarioService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _usuarios = database.GetCollection<Usuario>("usuarios");            
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
        public async Task<List<UsuarioDTO>> GetAllUsuarios()
        {
            List<UsuarioDTO> gusuario = new List<UsuarioDTO>();
            var añadir = new BsonDocument("$addFields",
   new BsonDocument("id_rol",
   new BsonDocument("$toObjectId", "$rol")));
            var ver = new BsonDocument("$lookup",
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
            var project = new BsonDocument("$project",
            new BsonDocument
                {
            { "_id", 1 },
            { "datos", 1 },
            { "estado", 1 },
            { "urol",
    new BsonDocument("nombre", 1) }
                });

            gusuario = await _usuarios.Aggregate()
                .AppendStage<dynamic>(añadir)
                .AppendStage<dynamic>(ver)
                .AppendStage<dynamic>(unwind)
                .AppendStage<UsuarioDTO>(project)
                .ToListAsync();
            return gusuario;
        }
    }
}
