using Autofac;
using Autofac.Integration.Mef;
using System;

namespace RauchTech.DataExtensions.AzureBlob.Builder
{

    public class AzureBlobModule : Module
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
                .RegisterType<AzureBlobService>()
                .As<IAzureBlobService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
