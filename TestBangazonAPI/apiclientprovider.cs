using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using BangazonAPI;
using System.Net.Http;

namespace BangazonAPITests
{

    class APIClientProvider : IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; private set; }
        private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();

        public APIClientProvider()
        {
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            Client?.Dispose();
        }
    }
}

