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
        private readonly VentaService _ventaservice;
        public PedidosService(ISisfahdDatabaseSettings settings, VentaService ventaService)
        {
            var pedidos = new MongoClient(settings.ConnectionString);
            var database = pedidos.GetDatabase(settings.DatabaseName);
            _PedidosCollection = database.GetCollection<Pedidos>("pedidos");
            _ventaservice = ventaService;
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

            //Crea la venta en pendiente para esa cita
            Venta venta = new Venta();
            venta.codigo_orden = "";
            venta.codigo_referencia = pedido.id;
            venta.monto = pedido.precio_neto;
            venta.tipo_pago = "Niubiz";
            venta.estado = "";
            venta.detalle_estado = "";
            venta.tipo_operacion = "Examenes";
            venta.titular = "";
            venta.moneda = "";

            _ventaservice.CrearVenta(venta);

            return pedido;
        }
        public Pedidos UpdatePedido(Pedidos pedido)
        {
            var filter = Builders<Pedidos>.Filter.Eq("id", pedido.id);
            var update = Builders<Pedidos>.Update
                .Set("paciente", pedido.paciente)
                .Set("tipo", pedido.tipo)
                .Set("id_acto_medico", pedido.id_acto_medico)
                .Set("productos", pedido.productos)
                .Set("estado_pago", pedido.estado_pago)
                .Set("fecha_creacion", pedido.fecha_creacion)
                .Set("fecha_pago", pedido.fecha_pago)
                .Set("precio_neto", pedido.precio_neto);

            _PedidosCollection.UpdateOne(filter, update);

            return pedido;
        }     
    }
}
