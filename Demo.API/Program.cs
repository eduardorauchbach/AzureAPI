using Autofac;
using Autofac.Extensions.DependencyInjection;
using Demo.API.Domain.Data.Repository.Builder;
using Demo.API.Domain.Service.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RauchTech.DataExtensions.Sql.Builder;
using System.Globalization;

namespace ATS.API
{
    public class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                    {
                        _ = builder.RegisterModule<ServiceModule>();
                        _ = builder.RegisterModule<RepositoryModule>();

                        _ = builder.RegisterModule<SqlModule>();                        
                    }))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
        }
    }
}
