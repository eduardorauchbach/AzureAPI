using Autofac;
using Autofac.Integration.Mef;
using System;

namespace RauchTech.DataExtensions.Sql.Builder
{

    public class SqlModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.Load(builder);
            builder.RegisterMetadataRegistrationSources();

            _ = builder
                .RegisterType<SqlHelper>()
                .As<ISqlHelper>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
