using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Linq;
using Elastic.Apm.NetCoreAll;
using EmployeeApi.Helpers;
using EmployeeApi.Repositories.Context;
using EmployeeApi.Services;
using EmployeeApi.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Repositories.Context;

namespace EmployeeApi
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
            services.AddCors();
            services.AddControllers();
            services.AddHttpContextAccessor();

            #region Dependency Injection

            //Common
            services.AddSingleton<DapperContext>();
            services.AddSingleton<Logging>();
            //services.AddSingleton<DBEmployeeContext>();

            //Service

            services.AddScoped<ILoginService, LoginService>();

            //Repository
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //services.AddDbContextPool<DBEmployeeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeConnectionStringSQL")));

            #endregion

            #region Swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "EmployeeApi", Version = "v1", Description = "EmployeeApi", });
                //options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "EmployeeApi", Version = "v2", Description = "EmployeeApi", });
                options.OperationFilter<SwaggerParameterFilters>();
                options.DocumentFilter<SwaggerVersionMapping>();

                options.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
                    var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
                    version = version.Replace("v", "");
                    return versions.Any(v => v.ToString() == version && maps.Any(v => v.ToString() == version));
                });
            });

            #endregion

            #region Api Versioning

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
                SwaggerConfig.UseCustomHeaderApiVersion("x-api-version");
            });

            #endregion

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger(options => options.RouteTemplate = "swagger/{documentName}/swagger.json");
                app.UseSwaggerUI(options =>
                {

                    options.DocumentTitle = "EmployeeApi";
                    options.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
                    //options.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v2");
                });
            }
            else
            {
                app.UseAllElasticApm(Configuration);
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}