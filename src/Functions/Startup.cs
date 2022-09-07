using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Configuration;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services;
using SFA.DAS.Configuration.AzureTableStorage;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var configBuilder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

#if DEBUG
            configBuilder.AddJsonFile("local.settings.json", optional: true);
#endif

            if (!configuration["EnvironmentName"].Equals("LOCAL_ACCEPTANCE_TESTS", StringComparison.CurrentCultureIgnoreCase))
            {
                configBuilder.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                });
            }

            var config = configBuilder.Build();
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));

            builder.Services.AddOptions();
            builder.Services.Configure<ApprenticeshipsApiOptions>(config.GetSection(ApprenticeshipsApiOptions.ApprenticeshipsApi));

            builder.Services.Configure<ApplicationSettings>(config.GetSection("ApplicationSettings"));
            var applicationSettings = config.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            Environment.SetEnvironmentVariable("NServiceBusConnectionString", applicationSettings.NServiceBusConnectionString);

            builder.Services.AddNServiceBus(applicationSettings);

            builder.Services.AddScoped<IApprenticeshipService, ApprenticeshipService>();
        }
    }
}
