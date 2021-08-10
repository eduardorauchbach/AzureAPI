using Autofac;
using Autofac.Integration.Mef;
using System;

namespace Demo.API.Domain.Service.Builder
{
    public class ServiceModule : Module
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
                .RegisterType<BlobFileService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<CandidateJobService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<CandidateService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<JobService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
