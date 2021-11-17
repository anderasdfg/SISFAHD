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
        private readonly IMongoCollection<Ordenes> _ordenes;
        public Turno_OrdenService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _turnosOr = database.GetCollection<Turno_Ordenes>("turnos_ordenes");
            _examen = database.GetCollection<Examenes>("examenes");
            _ordenes = database.GetCollection<Ordenes>("ordenes");
            _paciente = database.GetCollection<Paciente>("pacientes");
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
        public async Task<List<Turno_Ordenes>> GetBy_Especialidad_Fecha(Turno_OrdenDTO_By_Especialidad_Fecha consultas)
        {
            List<Turno_Ordenes> turnos = new List<Turno_Ordenes>();
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
                                            new BsonDocument("$gte",firstDate)),
                                            new BsonDocument("fecha_fin",
                                            new BsonDocument("$lte",lastDate)),
                                            new BsonDocument("especialidad.codigo", consultas.idespecialidad)
                                        }));

            turnos = await _turnosOr.Aggregate()
                .AppendStage<Turno_Ordenes>(match)
                .ToListAsync();

            return turnos;
        }

        public Turno_Ordenes Modificar_Turno_By_Reserva(Turno_OrdenDTO_By_Reserva turno)
        {
           
            Examenes examen = new Examenes();
            examen= _examen.Find(examen =>examen.id == turno.idExamen).FirstOrDefault();
            Turno_Ordenes turnosOrdenes = new Turno_Ordenes();
            turnosOrdenes = _turnosOr.Find(turnosOrdenes => turnosOrdenes.id == turno.idTurnoOrden).FirstOrDefault();
            DateTime fecha_hora_inicio = turnosOrdenes.fecha_inicio;
            string[] separador;
            int hora_separador;
            int minuto_separador;
            int contador = 0;
            int duracion_total = 0;
            if (turnosOrdenes.cupos is null)
            {
                turnosOrdenes.cupos = new List<CuposTO>();
                separador = turnosOrdenes.hora_inicio.Split(':');
                hora_separador = Convert.ToInt32(separador[0]);
                minuto_separador = Convert.ToInt32(separador[1]);
                fecha_hora_inicio.AddHours(hora_separador);
                fecha_hora_inicio.AddMinutes(minuto_separador);
                fecha_hora_inicio.AddSeconds(0);
            }
            else
            {
                foreach(CuposTO cupones in turnosOrdenes.cupos)
                {
                    duracion_total += cupones.duracion;
                    contador++;
                }
                double contador_inicio = duracion_total;
                //75 -->1.25 -->1 hora y 0.25 x60 =minutos
                double tiempo_añadir = (contador_inicio / 60);
                long hora_añadir = (long)tiempo_añadir;
                double decimales = tiempo_añadir - hora_añadir;
                double minutos_añadir = (decimales * 60);
                fecha_hora_inicio=fecha_hora_inicio.AddHours(Convert.ToInt32(hora_añadir));
                fecha_hora_inicio=fecha_hora_inicio.AddMinutes(Convert.ToInt32(minutos_añadir));
                fecha_hora_inicio=fecha_hora_inicio.AddSeconds(0);

            }
            Paciente pacienteReservado = new Paciente();
            pacienteReservado = _paciente.Find(paciente => paciente.id_usuario == turno.idUsuario).FirstOrDefault();
            
            turnosOrdenes.cupos.Add(new CuposTO() {
                hora_inicio=fecha_hora_inicio,
                paciente=Convert.ToString(pacienteReservado.id),
                duracion=Convert.ToInt32(examen.duracion),
                estado="ocupado",
                id_orden=turno.idOrden,
                id_examen= turno.idExamen
            });

            //Linea para modificar el turno_orden

            Ordenes ordenesReservado = new Ordenes();
            ordenesReservado = _ordenes.Find(ordenesReservado => ordenesReservado.id == turno.idOrden).FirstOrDefault();

            foreach (Procedimientos proced in ordenesReservado.procedimientos)
            {
                if (proced.id_examen == turno.idExamen)
                {
                    proced.id_turno_orden = turno.idTurnoOrden;
                }

            }
            return turnosOrdenes;
        }
    }
}
