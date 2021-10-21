using MongoDB.Driver;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Services
{
    public class PedidosService
    {
        private readonly IMongoCollection<Pedidos> _PedidosCollection;
        public PedidosService(ISisfahdDatabaseSettings settings)
        {
            var pedidos = new MongoClient(settings.ConnectionString);
            var database = pedidos.GetDatabase(settings.DatabaseName);
            _PedidosCollection = database.GetCollection<Pedidos>("pedidos");
        }
        public List<Pedidos> GetAll()
        {
            List<Pedidos> pedidos = new List<Pedidos>();
            pedidos = _PedidosCollection.Find(Pedido => true).ToList();
            return pedidos;
        }
        public Pedidos CreatePedido(Pedidos pedido)
        {
            _PedidosCollection.InsertOne(pedido);
            return pedido;
        }
    }
}
