using MongoDB.Bson;
using MongoDB.Driver;
using SISFAHD.DTOs;
using SISFAHD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace SISFAHD.Services
{
    public class ContraseñaService
    {

        private readonly IMongoCollection<Usuario> _UsuarioCollection;
        private static Random random = new Random();
        public ContraseñaService(ISisfahdDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _UsuarioCollection = database.GetCollection<Usuario>("usuarios");

        }

        // Verificar codigo
        public bool VerifyPass(string code)
        {
            Usuario user = new Usuario();
            user = _UsuarioCollection.Find(codigo => codigo.datos.codigo == code).FirstOrDefault();
            if (user == null)
            { return false; }
            else return true;
        }
        //Cambiar Pass
        public void ModificarPass(string code, string pass)
        {
            var filter = Builders<Usuario>.Filter.Eq("datos.codigo", code);
            var update = Builders<Usuario>.Update
                         .Set("clave", pass)
                         .Set("datos.codigo", "");
            _ = _UsuarioCollection.FindOneAndUpdateAsync<Usuario>(filter, update, new FindOneAndUpdateOptions<Usuario>
            {
                ReturnDocument = ReturnDocument.After
            });
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SendNotification(string email)
        {
            Usuario user = new Usuario();
            user = _UsuarioCollection.Find(sucorreo => sucorreo.usuario == email).FirstOrDefault();

            if (user == null) { return false; }
            else { 
            string codigorandom; // Cambiar por user.algo para asignarlo
            codigorandom = RandomString(12);

            var filter = Builders<Usuario>.Filter.Eq("datos.correo", email);
            var update = Builders<Usuario>.Update
                         .Set("datos.codigo", codigorandom);
            var resultado = _UsuarioCollection.FindOneAndUpdateAsync<Usuario>(filter, update, new FindOneAndUpdateOptions<Usuario>
            {
                ReturnDocument = ReturnDocument.After
            });

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            string Emisor = "sisfahdq@gmail.com";
            string EmisorPass = "sisf@hd12";
            string displayName = "SISFAHD";
            string Receptor = "keliv46623@ovooovo.com"; //user.usuario;
            string htmlbody = "<body style='margin:0;padding:0;'>" +
                                "<table role = 'presentation' style = 'width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;' >" +
                                        "<tr>" +
                                            "<td align = 'center' style = 'padding:40px 0 30px 0;background:#70bbd9;'>" +
                                                    "<img src = 'https://steemitimages.com/DQmWsqRZunt3hmk2hyCSTCjXwH92szLVP9XcNv9mnmPQL5N/password.png' alt = '' width = '300' style = 'height:auto;display:block;'/>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style = 'padding:0;background:#ee4c50;' >" +
                                                "SISFAHD" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style = 'padding:0;'>" +
                                                "<h1> Hola </h1>" +
                                                "<p> Hemos recibido una solicitud para acceder a tu cuenta:</p> " +
                                                "<p align=center style=color:blue> " + user.usuario + "</p>" +
                                                "<p> a través de tu dirección de correo electrónico. Tu código de verificación es: </p>" +
                                                "<hr>" +
                                                "<p align=center><strong>" + codigorandom + "</strong></p>" +
                                                "<hr>" +
                                                "<p> Atentamente, </p>" +
                                                "<p> El equipo de Cuentas de SISFAHD <br><br> </p>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td style = 'padding:0' align=center>" +
                                                "<a href = '' style = 'background-color:red; color:white; padding:1em 1em; text-transform:uppercase; text-decoration:none'>Recuperar</a>" +
                                             "</td>" +
                                        "</tr>" +
                                 "</table>" +
                               "</body> ";

            MailMessage mail = new MailMessage();
            mail.Subject = "Recupere su Contraseña";
            mail.From = new MailAddress(Emisor.Trim(), displayName);
            mail.Body = htmlbody;
            mail.To.Add(new MailAddress(Receptor));
            mail.IsBodyHtml = true;
            NetworkCredential nc = new NetworkCredential(Emisor, EmisorPass);
            smtp.Credentials = nc;
            smtp.Send(mail);
                return false;
            }
        }

    }
}
