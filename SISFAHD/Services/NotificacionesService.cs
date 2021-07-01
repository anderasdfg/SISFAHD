using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
using SISFAHD.Entities;

namespace SISFAHD.Services
{
    public class NotificacionesService
    {
        private readonly IMongoCollection<Paciente> _PacienteCollection;
        private readonly IMongoCollection<Usuario> _UsuarioCollection;
        private readonly IMongoCollection<Medico> _MedicoCollection;
        private readonly IMongoCollection<Cita> _CitaCollection;
        public NotificacionesService(ISisfahdDatabaseSettings settings)
        {
            var paciente = new MongoClient(settings.ConnectionString);
            var database = paciente.GetDatabase(settings.DatabaseName);
            _PacienteCollection = database.GetCollection<Paciente>("pacientes");
            _UsuarioCollection = database.GetCollection<Usuario>("usuarios");
            _CitaCollection = database.GetCollection<Cita>("citas");
            _MedicoCollection = database.GetCollection<Medico>("medicos");
        }
        public async Task<Cita> sendNotificacion(string idCita)
        {
            Cita c = new Cita();
            c =_CitaCollection.Find(cit => cit.id == idCita).FirstOrDefault();
            Paciente p = new Paciente();
            p = _PacienteCollection.Find(pacient => pacient.id == c.id_paciente).FirstOrDefault();
            Medico m = new Medico();
            m=_MedicoCollection.Find(med => med.id == c.id_medico).FirstOrDefault();
            Usuario objpaciente= new Usuario();
            objpaciente = _UsuarioCollection.Find(user => user.id == p.id_usuario).FirstOrDefault();
            Usuario objMedico = new Usuario();
            objMedico = _UsuarioCollection.Find(user => user.id == m.id_usuario).FirstOrDefault();
           
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            string Emisor = "SISFAHD@gmail.com";
            string EmisorPass = "******";
            string displayName = "SISFAHD";
            string Receptor = objpaciente.usuario;
            string htmlbody = "<body style='margin:0;padding:0;'>" +
"<table role = 'presentation' style = 'width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;' >" +
       "<tr>" +
           "<td align = 'center' style = 'padding:40px 0 30px 0;background:#70bbd9;'>" +
                    "<img src = 'https://blog.dinterweb.com/hubfs/directmailemail_1361936.jpg' alt = '' width = '300' style = 'height:auto;display:block;'/>" +
                     "</td>" +
                 "</tr>" +
                 "<tr>" +
                     "<td style = 'padding:0;'>" +
                          "<h1> Cita Pagada </h1>" +
                             "<p>Fecha de cita: " + c.fecha_cita.ToString() + "</p>" +
                             "<p>Médico: " + objMedico.datos.nombre + " " + objMedico.datos.apellido_paterno + " " + objMedico.datos.apellido_materno + "</p>" +
                         "</td>" +
                     "</tr>" +
                     "<tr>" +
                         "<td style = 'padding:0;background:#ee4c50;' >" +
                              "SISFAHD" +
                         "</td>" +
                      "</tr>" +
                  "</table>" +
                  "</body> ";

            MailMessage mail = new MailMessage();
            mail.Subject = "Bienvenido";
            mail.From = new MailAddress(Emisor.Trim(), displayName);
            mail.Body = htmlbody;
            mail.To.Add(new MailAddress(Receptor));
            mail.IsBodyHtml = true;
            NetworkCredential nc = new NetworkCredential(Emisor, EmisorPass);
            smtp.Credentials = nc;
            smtp.Send(mail);
            return c;
        }
    }
}
