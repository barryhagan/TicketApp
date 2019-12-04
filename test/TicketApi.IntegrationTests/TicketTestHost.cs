using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TicketApi.IntegrationTests
{
    public class TicketTestHost : IDisposable, IAsyncLifetime
    {
        public TestServer Server { get; }

        public TicketTestHost()
        {
            Server = new TestServer(CreateWebHostBuilder(new string[0]));
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseEnvironment("IntegrationTest")
                .UseStartup<TicketApi.Startup>();

        public T GetService<T>()
        {
            return (T)Server.Host.Services.GetService(typeof(T));
        }

        public HttpClient CreateClient() => Server.CreateClient();

        public void Dispose()
        {
            Server?.Dispose();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
