using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SISFAHD.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace SISFAHD.Services
{
    public class AdicionalesServices
    {
        private readonly IMongoCollection<Adicionales> _adicionales;
        public AdicionalesServices(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _adicionales = database.GetCollection<Adicionales>("adicionales");

        }

        public List<Adicionales> GetAll()
        {
            List<Adicionales> complementarios = new List<Adicionales>();
            complementarios = _adicionales.Find(Complmentario => true).ToList();
            return complementarios;
        }

        public Adicionales GetByTitulo(string titulo)
        {
            Adicionales complementario = new Adicionales();
            complementario = _adicionales.Find(complementario => complementario.titulo == titulo).FirstOrDefault();
            return complementario;
        }
        public Adicionales GetByID(string id)
        {
            Adicionales complementario = new Adicionales();
            complementario = _adicionales.Find(complementario => complementario.id == id).FirstOrDefault();
            return complementario;

        }

        public Adicionales ModificarAdicionaless(Adicionales complementario) {

            var filter = Builders<Adicionales>.Filter.Eq("id", complementario.id);
            var update = Builders<Adicionales>.Update
                            .Set("titulo", complementario.titulo)
                            .Set("descripcion", complementario.descripcion)
                            .Set("monto", complementario.monto)
                            .Set("url", complementario.url);
            complementario = _adicionales.FindOneAndUpdate<Adicionales>(filter, update, new FindOneAndUpdateOptions<Adicionales>
            {
                ReturnDocument = ReturnDocument.After
            });
            return complementario;
        }

        public Adicionales CrearAdicionales(Adicionales complementario) {

            _adicionales.InsertOne(complementario);
            return complementario;
        }

        public async Task<Adicionales> RemoveAdicioanles(string id)
        {
            var filter = Builders<Adicionales>.Filter.Eq("id", id);
            return await _adicionales.FindOneAndDeleteAsync<Adicionales>(filter, new FindOneAndDeleteOptions<Adicionales> { });
        }
    }
}
