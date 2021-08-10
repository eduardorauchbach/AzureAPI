using Autofac;
using Autofac.Integration.Mef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Domain.Data.Repository.Builder
{
    public class RepositoryModule : Module
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
                .RegisterType<CandidateJobRepository>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<CandidateRepository>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            _ = builder
                .RegisterType<JobRepository>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
