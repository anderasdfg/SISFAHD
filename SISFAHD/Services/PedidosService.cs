using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
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
            venta.tipo_operacion = pedido.tipo;
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
     
        public async Task<List<Pedidos>> GetByIdPaciente(string id_paciente)
        {
            List<Pedidos> pedidos = new List<Pedidos>();
            var match = new BsonDocument("$match",
                        new BsonDocument("paciente.id_paciente", id_paciente));
            pedidos = await _PedidosCollection.Aggregate().AppendStage<Pedidos>(match).ToListAsync();
            return pedidos;
        }
        public async Task<List<Pedidos>> GetByPacientePendiente(string id_paciente)
        {
            List<Pedidos> pedidos = new List<Pedidos>();
            var match = new BsonDocument("$match",
                        new BsonDocument
                        {
                            { "$or", new BsonArray {
                                new BsonDocument("estado_pago", "pagado"),
                                new BsonDocument("estado_pago", "en proceso")
                            } },
                            { "tipo", "Examenes" },
                            { "paciente.id_paciente", id_paciente }
                        });
            pedidos = await _PedidosCollection.Aggregate().AppendStage<Pedidos>(match).ToListAsync();
            return pedidos;
        }
        public async Task<List<PedidoDTO>> GetPacientesPedidosPendientes()
        {
            List<PedidoDTO> pacientes = new List<PedidoDTO>();
            var match = new BsonDocument("$match",
                        new BsonDocument
                        {
                            { "$or",
                            new BsonArray
                            {
                                new BsonDocument("estado_pago", "pagado"),
                                new BsonDocument("estado_pago", "pendiente")
                            } },
                            { "tipo", "Examenes" }
                        });
            var group = new BsonDocument("$group",
                        new BsonDocument
                        {
                            { "_id", "$paciente.id_paciente" },
                            { "cantidad", new BsonDocument("$sum", 1) }
                        });
            var addfields = new BsonDocument("$addFields",
                            new BsonDocument("id_paciente_object",
                            new BsonDocument("$toObjectId", "$_id")));
            var lookup = new BsonDocument("$lookup",
                            new BsonDocument
                            {
                                { "from", "pacientes" },
                                { "localField", "id_paciente_object" },
                                { "foreignField", "_id" },
                                { "as", "paciente" }
                            });
            var unwind = new BsonDocument("$unwind",
                            new BsonDocument
                            {
                                { "path", "$paciente" },
                                { "preserveNullAndEmptyArrays", true }
                            });
            var addfields1 = new BsonDocument("$addFields",
                             new BsonDocument("id_usuario_object",
                             new BsonDocument("$toObjectId", "$paciente.id_usuario")));
            var lookup1 = new BsonDocument("$lookup",
                            new BsonDocument
                            {
                                { "from", "usuarios" },
                                { "localField", "id_usuario_object" },
                                { "foreignField", "_id" },
                                { "as", "datos_usuario" }
                            });
            var unwind1 = new BsonDocument("$unwind",
                            new BsonDocument
                            {
                                { "path", "$datos_usuario" },
                                { "preserveNullAndEmptyArrays", true }
                            });
            var addfields2 = new BsonDocument("$addFields",
                                new BsonDocument
                                {
                                    { "datos_usua", "$datos_usuario.datos" },
                                    { "id_usuario", new BsonDocument("$toString", "$id_usuario_object") }});
            var project = new BsonDocument("$project",
                            new BsonDocument
                            {
                                { "id_paciente_object", 0 },
                                { "paciente._id", 0 },
                                { "paciente.antecedentes", 0 },
                                { "id_usuario_object", 0 },
                                { "datos_usuario", 0 }
                            });
            pacientes = await _PedidosCollection.Aggregate()
                        .AppendStage<dynamic>(match)
                        .AppendStage<dynamic>(group)
                        .AppendStage<dynamic>(addfields)
                        .AppendStage<dynamic>(lookup)
                        .AppendStage<dynamic>(unwind)
                        .AppendStage<dynamic>(addfields1)
                        .AppendStage<dynamic>(lookup1)
                        .AppendStage<dynamic>(unwind1)
                        .AppendStage<dynamic>(addfields2)
                        .AppendStage<PedidoDTO>(project).ToListAsync();
            return pacientes;
        }

        // Comprar Servicios Adicionales

        public Pedidos GetbyID(string id) {
            Pedidos pedidos = new Pedidos();
            pedidos = _PedidosCollection.Find(Pedido => Pedido.id == id).FirstOrDefault();
            return pedidos;
        }
        public async Task<List<Pedidos>> GetListProductos(string id)
        {

            List<Pedidos> productos = new List<Pedidos>();

            var filter = new BsonDocument("$match",
                                 new BsonDocument("_id",
                                     new ObjectId(id)));
            var Object = new BsonDocument("$project",
                                new BsonDocument("productos", 1));

            productos = await _PedidosCollection.Aggregate()
                                    .AppendStage<dynamic>(filter)
                                    .AppendStage<Pedidos>(Object)
                                    .ToListAsync();
            return productos;
        }

        public Pedidos UpdateProductos(Pedidos pedidos)
        {
            List<Pedidos> ped = new List<Pedidos>();
           
            var match = new BsonDocument("$match",
                                         new BsonDocument("_id",
                                         new ObjectId(pedidos.id)));
            var unwind = new BsonDocument("$unwind",
                                         new BsonDocument("path", "$productos"));
            var project = new BsonDocument("$project",
                                         new BsonDocument
                                                  {
                                                    { "productos", 1 },
                                                    { "value",
                                                                new BsonDocument("$multiply",
                                                                new BsonArray
                                                            {
                                                                new BsonDocument("$ifNull",new BsonArray {"$productos.precio",1}),
                                                                new BsonDocument("$ifNull",new BsonArray {"$productos.cantidad",1})
                                                    })}
                                         });
            var group = new BsonDocument("$group",
                                         new BsonDocument{
                                                           { "_id", "productos" },
                                                            // total
                                                           { "precio_neto", new BsonDocument("$sum", "$value")}});


            ped = _PedidosCollection.Aggregate()
                      .AppendStage<dynamic>(match)
                      .AppendStage<dynamic>(unwind)
                      .AppendStage<dynamic>(project)
                      .AppendStage<Pedidos>(group).ToList();

            var filter = Builders<Pedidos>.Filter.Eq("id", pedidos.id);
            var update = Builders<Pedidos>.Update
                         .Set("productos", pedidos.productos)
                         .Set("precio_neto", ped[0].precio_neto);
            _PedidosCollection.UpdateOne(filter, update);
            return pedidos;
        
        }

        public void EliminarPedido(string pedido,string codigo) {
            
            var pull = Builders<Pedidos>.Filter.Where(pedidos => pedidos.id.Equals(pedido));
            var update = Builders<Pedidos>.Update.PullFilter(codigo => codigo.productos, builder => builder.codigo == codigo);
             _PedidosCollection.UpdateOne(pull, update);
        }

        public async Task<List<Pedidos>> GetCarritoPaciente(string id_paciente) 
        {
            List<Pedidos> pedidos = new List<Pedidos>();
            var match = new BsonDocument("$match",
                        new BsonDocument{
                                            { "paciente.id_paciente", id_paciente },
                                            { "id_acto_medico", "" }});
            pedidos = await _PedidosCollection.Aggregate().AppendStage<Pedidos>(match).ToListAsync();
            return pedidos;
        }
    }
}
