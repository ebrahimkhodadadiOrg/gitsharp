using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Api.Config.Swagger
{
    public static class SwaggerExtension
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddTransient<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<EnumSchemaFilter>();

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMDkzNTMyODMyNTJAc2VwZWhyY2MuY29tIiwiSWQiOiIwOGVmNzA3MC1kYmVkLTQwNWYtYThiZC0yMmU3YmJiNzRlYmQiLCJTaG9wSWQiOiI4OTMwIiwiVXNlck9sZEd1aWQiOiIiLCJleHAiOjE2MjE4NTI1MzYsImlzcyI6Imh0dHA6Ly9zZXBlaHJjYy5jb20vIiwiYXVkIjoiaHR0cDovL3NlcGVocmNjLmNvbS8ifQ.jk3AsSzFtQcUcE2mEmA3JbdRA7IA2jtZNnWHhRB8O4U}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement {{securitySchema, new[] {"Bearer"}}};
                c.AddSecurityRequirement(securityRequirement);

                #region Versioning

                var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

                // Get all implemented API versions from the assembly
                var apiVersions = Assembly.GetEntryAssembly().GetTypes()
                    .Where(t => t.IsClass && t.Namespace != null && t.Namespace.Contains($"{assemblyName}.Controllers"))
                    .Select(t => t.GetCustomAttributes<ApiVersionAttribute>().FirstOrDefault())
                    .Distinct()
                    .Where(x => x != null && x.Versions != null)
                    .OrderByDescending(v => v?.Versions?.ToString());

                foreach (var version in apiVersions)
                {
                    var versionString = $"v{version.Versions.First().MajorVersion}";
                    if (version.Versions.First().Status != null)
                        versionString += $"-{version.Versions.First().Status}";
                    c.SwaggerDoc(versionString, new OpenApiInfo
                    {
                        Title = assemblyName,
                        Version = versionString
                    });
                }

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Remove version parameter from all Operations
                c.OperationFilter<RemoveVersionParameters>();

                //set version "api/v{version}/[controller]" from current swagger doc verion
                c.DocumentFilter<SetVersionInPaths>();

                //Seperate and categorize end-points by doc version
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });
                #endregion
            });
            
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/v{description.GroupName}/swagger.json", $"API v{description.GroupName}");
                }
            });
        }
    }
}