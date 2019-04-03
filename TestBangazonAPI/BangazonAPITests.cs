using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BangazonAPI;
using BangazonAPI.Models;
namespace BangazonAPITests
{
    public class CustomerTests
    {
        [Fact]
        public async Task GetCustomer_Success()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/Customer");
                response.EnsureSuccessStatusCode();

            }
        }
        [Fact]
        public async Task GetCustomers_Success()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/Customer/5");
                response.EnsureSuccessStatusCode();

            }
        }

        [Fact]
        public async Task GetCustomersq_Success()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/Customer?q=test");
                response.EnsureSuccessStatusCode();

            }
        }

        [Fact]
        public async Task PostCustomer_Success()
        {

            using (var client = new APIClientProvider().Client)
            {
                var customer = GenerateCustomer();
                var response = await client.PostAsync("api/Customer", new StringContent(customer, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

            }
        }

        private string GenerateCustomer()
        {
            var customer = new Customer
            {
                FirstName = "Marshal",
                LastName = "Lee"
            };
            return JsonConvert.SerializeObject(customer);
        }


        [Fact]
        public async Task PutCustomer_Succes()
        {

            using (var client = new APIClientProvider().Client)
            {
                var customer = GenerateCustomers();
                var response = await client.PutAsync("api/Customer/4", new StringContent(customer , Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

            }
        }


        private string GenerateCustomers()
        {
            var customer = new Customer
            {
                FirstName = "gabryella",
                LastName = "crawford"
            };
            return JsonConvert.SerializeObject(customer);
        }
    }
}