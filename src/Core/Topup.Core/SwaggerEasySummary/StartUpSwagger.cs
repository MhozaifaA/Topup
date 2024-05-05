

/*
 for more: https://github.com/MhozaifaA/SwaggerEasySummary
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Meteors;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SWSwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddMrSwagger(
            this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "2.0",
                    Title ="Topup App" ,//Assembly.GetEntryAssembly().GetName().Name,
                    Description = "API for " + Assembly.GetEntryAssembly().GetName().Name,
                   // TermsOfService = new Uri("https://localhost.com/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contact",
                      //  Url = new Uri("https://localhost.com/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                      //  Url = new Uri("https://localhost.com/")
                    }
                });


                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header. (not used in this app)"
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic",
                            }
                        },
                        new string[] {}
                    }
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                       new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "bearer"
                             }
                         },
                         new string[] {}
                    }});




                try
                {
                     var XmlDocPaths = System.IO.Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml");
                    var XmlDocs = (
                     from DocPath in XmlDocPaths select XDocument.Load(DocPath)
                   ).ToList();

                    foreach (var doc in XmlDocs)
                    {
                        try
                        {
                            options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
                            options.SchemaFilter<DescribeEnumMembers>(doc);
                        }
                        catch
                        {

                        }
                    }

                    options.DocumentFilter<EnumTypesDocumentFilter>();

                }
                catch
                {

                }



            });

            return services;
        }
    }
}


namespace Microsoft.AspNetCore.Builder
{
    public static class SWSwaggerUIBuilderExtensions
    {
        /// <summary>
        /// <para></para>
        /// c.DocExpansion(DocExpansion.None);
        /// <para></para>
        /// 
        /// c.InjectStylesheet(configuration["SubBasePath"] + "/app-swagger-ui.css");
        /// <para></para>
        /// 
        /// c.InjectJavascript(configuration["SubBasePath"] + "/jquery-3.6.0.min.js");
        /// <para></para>
        ///
        /// c.InjectJavascript(configuration["SubBasePath"] + "/app-swagger-ui.js");
        /// </summary>
        /// <param name="app"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMrSwagger(this IApplicationBuilder app, Action<SwaggerUIOptions> setupAction = null)
        {
           
            IConfiguration configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

            if (!configuration.GetValue<bool>("EnableSwagger"))
                return app;

            app.UseSwagger();
            Action<SwaggerUIOptions> action = c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.InjectStylesheet(configuration["SubBasePath"] + "/app-swagger-ui.css");
                //c.InjectJavascript(configuration["SubBasePath"] + "/jquery.min.js");
                c.InjectJavascript(configuration["SubBasePath"] + "/app-swagger-ui.js");
            };
            setupAction ??= action;



            return app.UseSwaggerUI(setupAction);
        }
    }
}
