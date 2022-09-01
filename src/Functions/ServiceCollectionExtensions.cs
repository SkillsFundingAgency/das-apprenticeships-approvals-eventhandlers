﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.AzureFunction.Hosting;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions
{
    [ExcludeFromCodeCoverage]
    public static class NServiceBusStartupExtensions
    {
        public static IServiceCollection AddNServiceBus(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var webBuilder = serviceCollection.AddWebJobs(x => { });
            webBuilder.AddExecutionContextBinding();
            webBuilder.AddExtension(new NServiceBusExtensionConfigProvider());

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.Apprenticeships")
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer();

            if (configuration["ApplicationSettings:NServiceBusConnectionString"].Equals("UseLearningEndpoint=true", StringComparison.CurrentCultureIgnoreCase))
            {
                endpointConfiguration
                    .UseTransport<LearningTransport>()
                    .StorageDirectory(configuration.GetValue("ApplicationSettings:LearningTransportStorageDirectory",
                        Path.Combine(
                            Directory.GetCurrentDirectory()
                                .Substring(0, Directory.GetCurrentDirectory().IndexOf("src")),
                            @"src\SFA.DAS.Apprenticeships.Functions.TestConsole\.learningtransport")));
                endpointConfiguration.UseLearningTransport(s => s.AddRouting());
            }
            else
            {
                endpointConfiguration
                    .UseAzureServiceBusTransport(configuration["ApplicationSettings:NServiceBusConnectionString"], r => r.AddRouting());
            }

            if (!string.IsNullOrEmpty(configuration["ApplicationSettings:NServiceBusLicense"]))
            {
                endpointConfiguration.License(configuration["ApplicationSettings:NServiceBusLicense"]);
            }

            ExcludeTestAssemblies(endpointConfiguration.AssemblyScanner());
            
            var endpointWithExternallyManagedServiceProvider = EndpointWithExternallyManagedServiceProvider.Create(endpointConfiguration, serviceCollection);
            endpointWithExternallyManagedServiceProvider.Start(new UpdateableServiceProvider(serviceCollection));
            serviceCollection.AddSingleton(p => endpointWithExternallyManagedServiceProvider.MessageSession.Value);

            return serviceCollection;
        }

        private static void ExcludeTestAssemblies(AssemblyScannerConfiguration scanner)
        {
            var excludeRegexs = new List<string>
            {
                @"nunit.*.dll"
            };

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var fileName in Directory.EnumerateFiles(baseDirectory, "*.dll")
                         .Select(Path.GetFileName))
            {
                foreach (var pattern in excludeRegexs)
                {
                    if (Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase))
                    {
                        scanner.ExcludeAssemblies(fileName);
                        break;
                    }
                }
            }
        }
    }
}