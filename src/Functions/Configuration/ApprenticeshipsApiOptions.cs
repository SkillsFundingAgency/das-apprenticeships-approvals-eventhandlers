﻿using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ApprenticeshipsApiOptions : IApimClientConfiguration
    {
        public const string ApprenticeshipsApi = "ApprenticeshipsApi";
        public string ApiBaseUrl { get; set; }
        public string SubscriptionKey { get; set; }
        public string ApiVersion { get; set; }
    }
}
