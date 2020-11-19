using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.API.Data;

namespace SmartSchool.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default")));

            // Cria uma �nica inst�ncia do servi�o quando � solicitado pela primeira
            // vez e reutiliza essa mesma inst�ncia em todos os locais em que esse
            // servi�o � necess�rio
            // ==>>>  services.AddSingleton<IRepository, Repository>();

            // Sempre vai gerar uma nova inst�ncia para cada item encontrado que
            // possua tal depend�ncia, ou seja, se houver 5 depend�ncias, ser�o 5 inst�ncias
            // ==>>>  services.AddTransient<IRepository, Repository>();

            // Garante que em uma requisi��o seja criada uma inst�ncia de uma classe se
            // houver outras depend�ncias (renovando nas requisi��es subsequentes)
            // ==>>>  services.AddScoped<IRepository, Repository>();


            services.AddControllers()
                    .AddNewtonsoftJson(
                        opt => opt.SerializerSettings.ReferenceLoopHandling = 
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepository, Repository>();

            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            var apiProviderDescription = services.BuildServiceProvider()
                                            .GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options =>
            {

                foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        new Microsoft.OpenApi.Models.OpenApiInfo()
                        {
                            Title = "SmartSchool API",
                            Version = description.ApiVersion.ToString(),
                            TermsOfService = new Uri("http://qualquernome.com"),
                            License = new Microsoft.OpenApi.Models.OpenApiLicense
                            {
                                Name = "Licen�a Qualquer",
                                Url = new Uri("http://licencaqualquer.com")
                            },
                            Contact = new Microsoft.OpenApi.Models.OpenApiContact
                            {
                                Name = "Adriana Barranco",
                                Email = "abarranco3012@gmail.com"
                            }
                        });
                }
                

                var xmlComentario = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCaminho = Path.Combine(AppContext.BaseDirectory, xmlComentario);

                options.IncludeXmlComments(xmlCaminho);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                                IWebHostEnvironment env,
                                IApiVersionDescriptionProvider apiProviderDescription)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger().UseSwaggerUI(options =>
            {
                foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                
                options.RoutePrefix = "";
            });

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
