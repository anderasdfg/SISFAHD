using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SISFAHD.Helpers;
using SISFAHD.Services;
using Microsoft.OpenApi.Models;

namespace SISFAHD
{
    public class Startup
    {
        public IConfiguration Configuration;
        readonly string PolizaCORSSISFAHD = "_polizaCORSSISFAHD";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //cualquier cliente desplegado localmente en el puerto
            //logico 8080 podra consumir las APIS del SISFAHD_BACK
            services.AddCors(options =>
            {
                options.AddPolicy(name: PolizaCORSSISFAHD,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:8080")
                                                .AllowAnyMethod()
                                                .AllowAnyHeader();
                                  });
            });

            services.AddControllers();

            //Configurando dependencia de Clase conector con MongoDB
            services.Configure<SisfahdDatabaseSettings>(
                Configuration.GetSection(nameof(SisfahdDatabaseSettings)));

            services.AddSingleton<ISisfahdDatabaseSettings>(sp =>
              sp.GetRequiredService<IOptions<SisfahdDatabaseSettings>>().Value);
            //Agregando Poliza CORS
            services.AddCors();
            //Inyectando dependencia de Clase Conectora en la Interfaz padre
            services.AddSingleton<ISisfahdDatabaseSettings>(sp =>
               sp.GetRequiredService<IOptions<SisfahdDatabaseSettings>>().Value);
            //Inyección de dependencias       
            services.AddScoped<UsuarioService>();
            services.AddScoped<CitaService>();
            services.AddScoped<PacienteService>();
            services.AddScoped<TurnoService>();
            services.AddScoped<EspecialidadService>();
            services.AddScoped<MedicoService>();
            services.AddScoped<MedicamentoService>();
            services.AddScoped<TarifaService>();
            services.AddScoped<ActoMedicoService>();
            services.AddScoped<VentaService>();
            services.AddScoped<HistoriaService>();
            services.AddScoped<NotificacionesService>();
            services.AddScoped<EnfermedadesService>();
            services.AddScoped<ProcedimientoService>();
            services.AddScoped<MedicinasServices>();
            services.AddScoped<PedidosService>();
            services.AddScoped<Turno_OrdenService>();
            services.AddScoped<ContraseñaService>();
            services.AddScoped<EstadisticaService>();
            services.AddScoped<CalificacionService>();
            services.AddScoped<AdicionalesServices>();
            services.AddScoped<ExamenesService>();
            services.AddScoped<ResultadoExamenService>();
            services.AddScoped<MediaService>();
            services.AddScoped<OpinionesService>();
            services.AddScoped<OrdenesService>();
            //Injectando dependecia de Azure FileStorage
            services.AddScoped<IFileStorage, AzureFileStorage>();

            //need default token provider
            services.AddAuthentication(JwtBearerDefaults
             .AuthenticationScheme)
                 .AddJwtBearer(options =>
              options.TokenValidationParameters =
              new TokenValidationParameters
              {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(
                 //llave secreta que dice si el token es valido
                 Encoding.UTF8.GetBytes(Configuration.GetSection("jwt:key").Value)),
                  ClockSkew = TimeSpan.Zero
              });

            services.AddRazorPages();

            // Se encarga de registrar el generador del swagger
            services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "version 1.0",
                    Title = "SISFAHD API",
                    Description = "Aplicación que contiene la descripci�n , parametros, uso  y otras especificaciones de las APIS del SISFAHD"
                });

                g.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autorizaci�n para la entradas a las apis que generan la cabecera",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                g.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {  new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                   }
                });

            });

            services.AddMvc();
            //services.AddM
            //MvcOptions.EnabledEndpointRouting = false;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            //configuracion de la poliza Cross Origin Request Side
            app.UseCors(PolizaCORSSISFAHD);

            //Habilita el uso del swagger
            app.UseSwagger();

            //Habilita el uso de la interfaz del swagger
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "SISFAHD API V1.0");
            });

            //Autorizacion y auntenticacion mediante tokens JWT
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            

            //app.UseMvc(routes =>
            //{
            //    // You can add all the routes you need here

            //    // And the default route :
            //    routes.MapRoute(
            //         name: "default_route",
            //         template: "{controller}/{action}/{id?}",
            //         defaults: new { controller = "Home", action = "Index" }
            //    );
            //});
        }
    }
}
