﻿using NServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Helpers;

public static class EndpointHelper  
{
    public static async Task<IEndpointInstance> StartEndpoint(string endpointName, bool isSendOnly, Type[] types)
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);
        endpointConfiguration.AssemblyScanner().ThrowExceptions = false;

        if(isSendOnly) endpointConfiguration.SendOnly();

        endpointConfiguration.UseNewtonsoftJsonSerializer();
        endpointConfiguration.Conventions().DefiningEventsAs(types.Contains);

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        transport.StorageDirectory(Path.Combine(Directory.GetCurrentDirectory()[..Directory.GetCurrentDirectory().IndexOf("src", StringComparison.Ordinal)], @"src\.learningtransport"));
        transport.Routing().AddRouting();

        return await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
    }
}