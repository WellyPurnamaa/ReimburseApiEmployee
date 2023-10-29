using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
using EmployeeApi.Models;

namespace EmployeeApi.Helpers
{
    public class Logging
    {
        public void CreateLog(LogModel log, Exception ex = null)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true)
                .Build();

            string urlElastic = configuration["ElasticConfiguration:Uri"];
            urlElastic = urlElastic.Contains("http") ? urlElastic : $"http://{urlElastic}";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(urlElastic))
                {
                    AutoRegisterTemplate = false,
                    IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{environment}-{DateTime.UtcNow:yyyy-MM-dd}"
                })
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                if (log.LogType == Constant.LOG_INFO)
                {
                    Log.ForContext("LogName", log.LogName)
                        .ForContext("Reference", log.Reference)
                        .ForContext("Request", log.Request)
                        .ForContext("Response", log.Response)
                        .ForContext("Url", log.Url)
                        .ForContext("RequestPath", log.RequestPath)
                        .ForContext("ResponseTime", log.ResponseTime)
                        .ForContext("Environment", environment)
                        .Information(log.Message);
                }
                else if (log.LogType == Constant.LOG_WARNING)
                {
                    Log.ForContext("LogName", log.LogName)
                        .ForContext("Reference", log.Reference)
                        .ForContext("Request", log.Request)
                        .ForContext("Response", log.Response)
                        .ForContext("Url", log.Url)
                        .ForContext("RequestPath", log.RequestPath)
                        .ForContext("ResponseTime", log.ResponseTime)
                        .ForContext("Environment", environment)
                        .Warning(log.Message);
                }
                else if (log.LogType == Constant.LOG_ERROR)
                {
                    Log.ForContext("LogName", log.LogName)
                        .ForContext("Reference", log.Reference)
                        .ForContext("Request", log.Request)
                        .ForContext("Response", log.Response)
                        .ForContext("Url", log.Url)
                        .ForContext("RequestPath", log.RequestPath)
                        .ForContext("ResponseTime", log.ResponseTime)
                        .ForContext("Environment", environment)
                        .Error(log.Message);
                }
                else
                {
                    Log.ForContext("LogName", log.LogName)
                        .ForContext("Reference", log.Reference)
                        .ForContext("Request", log.Request)
                        .ForContext("Response", log.Response)
                        .ForContext("Url", log.Url)
                        .ForContext("RequestPath", log.RequestPath)
                        .ForContext("ResponseTime", log.ResponseTime)
                        .ForContext("Environment", environment)
                        .Fatal(ex, ex.Message);
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
