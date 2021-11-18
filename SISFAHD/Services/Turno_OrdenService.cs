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
    public class Turno_OrdenService
    {
        private readonly IMongoCollection<Turno_Ordenes> _turnosOr;
        private readonly IMongoCollection<Examenes> _examen;
        private readonly IMongoCollection<Paciente> _paciente;
        private readonly IMongoCollection<Medico> _medico;
        private readonly IMongoCollection<Ordenes> _ordenes;
        private readonly IMongoCollection<Usuario> _usuarios;

        public Turno_OrdenService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _turnosOr = database.GetCollection<Turno_Ordenes>("turnos_ordenes");
            _examen = database.GetCollection<Examenes>("examenes");
            _ordenes = database.GetCollection<Ordenes>("ordenes");
            _paciente = database.GetCollection<Paciente>("pacientes");
            _medico = database.GetCollection<Medico>("medicos");
            _usuarios = database.GetCollection<Usuario>("usuarios");

        }
        public List<Turno_Ordenes> GetAll()
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            turnos = _turnosOr.Find(Turnos => true).ToList();
            return turnos;
        }
        public Turno_Ordenes CreateTurno(Turno_Ordenes turno)
        {
            _turnosOr.InsertOne(turno);
            return turno;
        }
        public async Task<List<Turno_Ordenes>> GetByMedico(string idMedico, int month, int year)
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            DateTime firstDate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime lastDate = firstDate.AddMonths(2).AddDays(-1);
            lastDate.AddHours(23);
            lastDate.AddMinutes(59);
            lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_inicio",
                                            new BsonDocument("$gte",firstDate)),
                                            new BsonDocument("fecha_fin",
                                            new BsonDocument("$lte",lastDate)),
                                            new BsonDocument("id_medico", idMedico)
                                        }));

            turnos = await _turnosOr.Aggregate()
                .AppendStage<Turno_Ordenes>(match)
                .ToListAsync();

            return turnos;
        }
        public Turno_Ordenes ModifyTurno(Turno_Ordenes turno)
        {
            var filter = Builders<Turno_Ordenes>.Filter.Eq("id", turno.id);
            var update = Builders<Turno_Ordenes>.Update
                .Set("especialidad", turno.especialidad)
                .Set("estado", turno.estado)
                .Set("fecha_fin", turno.fecha_fin)
                .Set("fecha_inicio", turno.fecha_inicio)
                .Set("hora_fin", turno.hora_fin)
                .Set("hora_inicio", turno.hora_inicio)
                .Set("id_medico", turno.id_medico)
                .Set("cupos", turno.cupos);

            _turnosOr.UpdateOne(filter, update);
            return turno;
        }
        public async Task<Turno_Ordenes> DeleteTurno(string id)
        {
            var filter = Builders<Turno_Ordenes>.Filter.Eq("id", id);
            return await _turnosOr.FindOneAndDeleteAsync<Turno_Ordenes>
                        (filter, new FindOneAndDeleteOptions<Turno_Ordenes>{});
        }
        public Turno_Ordenes GetById(string id)
        {
            Turno_Ordenes turno = new Turno_Ordenes();
            turno = _turnosOr.Find(turno => turno.id == id).FirstOrDefault();
            return turno;
        }
        public async Task<List<Tuple< Turno_Ordenes,Usuario >>> GetBy_Especialidad_Fecha(Turno_OrdenDTO_By_Especialidad_Fecha consultas)
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            List<Tuple< Turno_Ordenes,Usuario>> tuple = new List<Tuple<Turno_Ordenes, Usuario>>();

            DateTime firstDate = new DateTime(consultas.año, consultas.mes, consultas.dia, 0, 0, 0);
            DateTime lastDate = firstDate;
            lastDate.AddHours(23);
            lastDate.AddMinutes(59);
            lastDate.AddSeconds(59);

            var match = new BsonDocument("$match",
                                new BsonDocument("$and",
                                new BsonArray
                                        {
                                            new BsonDocument("fecha_inicio",
                                            new BsonDocument("$lte",firstDate)),
                                            
                                            new BsonDocument("fecha_fin",
                                            new BsonDocument("$gte",lastDate)),
                                            new BsonDocument("especialidad.codigo", consultas.idespecialidad)
                                        }));

            turnos = await _turnosOr.Aggregate()
                .AppendStage<Turno_Ordenes>(match)
                .ToListAsync();
            Medico medico = new Medico();
            Usuario usuario = new Usuario();
            foreach (Turno_Ordenes turnOrdenes in turnos)
            {
                
                medico = _medico.Find(medico => medico.id == turnOrdenes.id_medico).FirstOrDefault();
                usuario = _usuarios.Find(usuario => usuario.id == medico.id_usuario).FirstOrDefault();
                Tuple<Turno_Ordenes, Usuario> tuple1 = new Tuple<Turno_Ordenes, Usuario>(turnOrdenes, usuario);
                tuple.Add(tuple1);
            }
            return tuple;
        }

        public Turno_Ordenes Modificar_Turno_By_Reserva(Turno_OrdenDTO_By_Reserva turno)
        {
           
            Examenes examen = new Examenes();
            examen= _examen.Find(examen =>examen.id == turno.idExamen).FirstOrDefault();
            Turno_Ordenes turnosOrdenes = new Turno_Ordenes();
            turnosOrdenes = _turnosOr.Find(turnosOrdenes => turnosOrdenes.id == turno.idTurnoOrden).FirstOrDefault();
            DateTime fecha_hora_inicio = turnosOrdenes.fecha_inicio;
            //DateTime fecha_hora_fin = turnosOrdenes.fecha_fin;
            string[] separador;
            int hora_separador;
            int minuto_separador;
            int contador = 0;
            int duracion_total = 0;
            separador = turnosOrdenes.hora_inicio.Split(':');
            hora_separador = Convert.ToInt32(separador[0]);
            minuto_separador = Convert.ToInt32(separador[1]);

            if (turnosOrdenes.cupos is null)
            {
                turnosOrdenes.cupos = new List<CuposTO>();
                fecha_hora_inicio = new DateTime(turno.fecha.Year, turno.fecha.Month, turno.fecha.Day, hora_separador, minuto_separador, 0);
                fecha_hora_inicio = fecha_hora_inicio.AddHours(-5);
            }
            else
            {

                foreach(CuposTO cupones in turnosOrdenes.cupos)
                {
                    if (cupones.hora_inicio.ToShortDateString() != turno.fecha.ToShortDateString())
                    {
                        duracion_total += 0;
                    }
                    else
                    {
                        duracion_total += cupones.duracion;
                        contador++;
                    }
                    
                }
                double contador_inicio = duracion_total;
                fecha_hora_inicio = new DateTime(turno.fecha.Year, turno.fecha.Month, turno.fecha.Day, hora_separador, minuto_separador, 0);
                fecha_hora_inicio=fecha_hora_inicio.AddMinutes(Convert.ToInt32(contador_inicio));
                fecha_hora_inicio = fecha_hora_inicio.AddHours(-5);

            }
            Paciente pacienteReservado = new Paciente();
            pacienteReservado = _paciente.Find(paciente => paciente.id_usuario == turno.idUsuario).FirstOrDefault();
            
            turnosOrdenes.cupos.Add(new CuposTO() {
                hora_inicio=fecha_hora_inicio,
                paciente=Convert.ToString(pacienteReservado.id),
                duracion=Convert.ToInt32(examen.duracion),
                estado="no pagado",
                id_orden=turno.idOrden,
                id_examen= turno.idExamen
            });

            //Linea para modificar el turno_orden
            var filter = Builders<Turno_Ordenes>.Filter.Eq("id", ObjectId.Parse(turnosOrdenes.id));
            var update = Builders<Turno_Ordenes>.Update
                .Set("cupos", turnosOrdenes.cupos);
            _turnosOr.UpdateOne(filter, update);


            Ordenes ordenesReservado = new Ordenes();
            ordenesReservado = _ordenes.Find(ordenesReservado => ordenesReservado.id == turno.idOrden).FirstOrDefault();

            foreach (Procedimientos proced in ordenesReservado.procedimientos)
            {
                if (proced.id_examen == turno.idExamen)
                {
                    proced.id_turno_orden = turno.idTurnoOrden;
                }

            }
            Console.WriteLine(ordenesReservado);
            //Linea para modificar ordenes
            var filteOrden = Builders<Ordenes>.Filter.Eq("id", ObjectId.Parse(ordenesReservado.id));
            var updateOrden = Builders<Ordenes>.Update
                .Set("procedimientos", ordenesReservado.procedimientos);
            _ordenes.UpdateOne(filteOrden, updateOrden);
            Console.WriteLine(turnosOrdenes);
            return turnosOrdenes;
        }
        public async Task<List<Turno_Ordenes>> GetByIdMedicoAsync(string id_medico)
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
            var match = new BsonDocument("$match",
                        new BsonDocument
                        {
                            { "id_medico", id_medico },
                            { "estado", "pendiente" }
                        });
            turnos = await _turnosOr.Aggregate()
                    .AppendStage<Turno_Ordenes>(match)
                    .ToListAsync();
            return turnos;
        }

        public async Task<List<PedidoDTO>> GetPacientebyExamenesPendientes(string id_medico)
        {
            List<PedidoDTO> pacientes = new List<PedidoDTO>();
            var match = new BsonDocument("$match",
                         new BsonDocument("id_medico", id_medico));
            var unwind = new BsonDocument("$unwind",
                         new BsonDocument
                            {
                                { "path", "$cupos" },
                                { "preserveNullAndEmptyArrays", false }
                            });
            var match1 = new BsonDocument("$match",
                            new BsonDocument("cupos.estado", "pagado"));
            var group = new BsonDocument("$group",
                        new BsonDocument
                        {
                            { "_id", "$cupos.paciente" },
                            { "cantidad",
                                new BsonDocument("$sum", 1) }
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
            var unwind1 = new BsonDocument("$unwind",
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
            var unwind2 = new BsonDocument("$unwind",
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
            pacientes = await _turnosOr.Aggregate()
                        .AppendStage<dynamic>(match)
                        .AppendStage<dynamic>(unwind)
                        .AppendStage<dynamic>(match1)
                        .AppendStage<dynamic>(group)
                        .AppendStage<dynamic>(addfields)
                        .AppendStage<dynamic>(lookup)
                        .AppendStage<dynamic>(unwind1)
                        .AppendStage<dynamic>(addfields1)
                        .AppendStage<dynamic>(lookup1)
                        .AppendStage<dynamic>(unwind2)
                        .AppendStage<dynamic>(addfields2)
                        .AppendStage<PedidoDTO>(project).ToListAsync();
            return pacientes;
        }
    }
}
