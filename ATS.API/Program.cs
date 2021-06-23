using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Globalization;
using System.IO;

namespace ATS.API
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            //ConfigurationBuilder builder = new ConfigurationBuilder();
            //BuildConfig(builder);

            ////Definir onde salvar logs

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(builder.Build())
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .CreateLogger();

            //Log.Logger.Information("Application Starting");

            CreateHostBuilder(args).Build().Run();
        }

        //private static void BuildConfig(IConfigurationBuilder builder)
        //{
        //    builder.SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Candidateion"}.json", optional: true)
        //        .AddEnvironmentVariables();
        //}

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
