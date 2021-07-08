using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class HistoriaService
    {
        private readonly IMongoCollection<Historia> _historias;
        public HistoriaService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _historias = database.GetCollection<Historia>("historias");
        }
        public Historia GetById(string id)
        {
            Historia historia = new Historia();
            historia = _historias.Find(historia => historia.id == id).FirstOrDefault();
            return historia;
        }

        public Historia ModifyHistoria(Historia historia)
        {
            var filter = Builders<Historia>.Filter.Eq("id", historia.id);
            var pushHistoria = Builders<Historia>.Update.Push("historial", historia.historial[0]);
             _historias.FindOneAndUpdateAsync(filter, pushHistoria);
            return historia;
        }        
    }
}
