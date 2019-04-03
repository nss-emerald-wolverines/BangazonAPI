using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BangazonAPI;

namespace BangazonAPItTests
{
    public class CustomerTests
    {
        [Fact]
        public async Task GetCustomer_Succes()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/Customer");
                response.EnsureSuccessStatusCode();

            }
        }
        [Fact]
        public async Task GetCustomers_Succes()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/Customer/5");
                response.EnsureSuccessStatusCode();

            }
        }

        [Fact]
        public async Task GetCustomersq_Succes()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("api/Customer?q=test");
                response.EnsureSuccessStatusCode();

            }
        }

        [Fact]
        public async Task PostCustomer_Succes()
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
                FirstName = "Marshal",
                LastName = "Lee"
            };
            return JsonConvert.SerializeObject(customer);
        }
    }
}