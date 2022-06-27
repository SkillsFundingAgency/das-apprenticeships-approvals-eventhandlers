using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Hooks;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Configuration;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services
{
    public class TestApprovalsFunctions : IDisposable
    {
        private readonly TestContext _testContext;
        private readonly TestEarningsApi _testEarningsApi;
        private readonly Dictionary<string, string> _appConfig;
        private readonly Dictionary<string, string> _hostConfig;
        private readonly TestMessageBus? _testMessageBus;
        private readonly List<IHook> _messageHooks;
        private IHost? _host;
        private bool _isDisposed;
        
        public TestApprovalsFunctions(TestContext testContext)
        {
            _testContext = testContext;
            _testEarningsApi = testContext.EarningsApi;
            _testMessageBus = testContext.TestMessageBus;
            _messageHooks = testContext.Hooks;

            _hostConfig = new Dictionary<string, string>();
            _appConfig = new Dictionary<string, string>
            {
                { "EnvironmentName", "LOCAL_ACCEPTANCE_TESTS" },
                { "ConfigurationStorageConnectionString", "UseDevelopmentStorage=true" },
                { "ConfigNames", "SFA.DAS.EmployerIncentives.Functions" },
                { "NServiceBusConnectionString", "UseDevelopmentStorage=true" },
                { "AzureWebJobsStorage", "UseDevelopmentStorage=true" }
            };
        }

        public async Task Start()
        {
            var startUp = new Startup();

            var hostBuilder = new HostBuilder()
                    .ConfigureHostConfiguration(a =>
                    {
                        a.Sources.Clear();
                        a.AddInMemoryCollection(_hostConfig);
                    })
                    .ConfigureAppConfiguration(a =>
                    {
                        a.Sources.Clear();
                        a.AddInMemoryCollection(_appConfig);
                        a.SetBasePath(_testMessageBus?.StorageDirectory?.FullName);
                    })
                    .ConfigureWebJobs(startUp.Configure)
                ;

            _ = hostBuilder.ConfigureServices((s) =>
            {
                s.Configure<ApprenticeshipsApiOptions>(a =>
                {
                    a.ApiBaseUrl = _testEarningsApi.BaseAddress;
                    a.SubscriptionKey = "";
                });
                
                _ = s.AddNServiceBus(new LoggerFactory().CreateLogger<TestApprovalsFunctions>(),
                    (o) =>
                    {
                        o.EndpointConfiguration = (endpoint) =>
                        {
                            endpoint.UseTransport<LearningTransport>().StorageDirectory(_testMessageBus?.StorageDirectory?.FullName);
                            return endpoint;
                        };

                        var hook = _messageHooks.SingleOrDefault(h => h is Hook<MessageContext>) as Hook<MessageContext>;
                        if (hook != null)
                        {
                            o.OnMessageReceived = (message) =>
                            {
                                hook.OnReceived?.Invoke(message);
                            };
                            o.OnMessageProcessed = (message) =>
                            {
                                hook.OnProcessed?.Invoke(message);
                            };
                            o.OnMessageErrored = (exception, message) =>
                            {
                                hook.OnErrored?.Invoke(exception, message);
                            };
                        }
                    });
            });

            hostBuilder.UseEnvironment("LOCAL");
            _host = await hostBuilder.StartAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _host?.StopAsync();
            }
            _host?.Dispose();

            _host?.Dispose();

            _isDisposed = true;
        }
    }
}
