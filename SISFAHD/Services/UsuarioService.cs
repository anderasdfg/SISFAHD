using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;


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
    }
}
