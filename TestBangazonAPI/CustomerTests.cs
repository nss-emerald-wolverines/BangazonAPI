using BangazonAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace TestBangazonAPI
{
    public class CustomerTests
    {
        [Fact]
        public async Task GetCustomer_Success()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.GetAsync("api/Customer");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var CustomerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(CustomerList.Count > 0);
            }

        }


        [Fact]
        public async Task GetCustomers_Success()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.GetAsync("api/Customer/5");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var CustomerList = JsonConvert.DeserializeObject<Customer>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(CustomerList.Id == 5);
            }
        }

        [Fact]
        public async Task GetCustomersq_Success()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {
                HttpResponseMessage response = await client.GetAsync("api/Customer?q=mir");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var CustomerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(CustomerList.Count > 0);
            }

        }

        [Fact]
        public async Task PostCustomer_Success()
        {

            using (HttpClient client = new APIClientProvider().Client)
            {
                string customer = GenerateCustomer();
                HttpResponseMessage response = await client.PostAsync("api/Customer", new StringContent(customer, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Customer DeJSONCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);
                Assert.Equal("Marshal", DeJSONCustomer.FirstName);
                Assert.Equal("Lee", DeJSONCustomer.LastName);
            }
        }
        private string GenerateCustomer()
        {
            Customer customer = new Customer
            {
                FirstName = "Marshal",
                LastName = "Lee"
            };
            return JsonConvert.SerializeObject(customer);
        }
        [Fact]
        public async Task PutCustomer_Success()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                string customer = GenerateCustomerPut();
                HttpResponseMessage response = await client.PutAsync("api/Customer/4", new StringContent(customer, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            }
        }
        private string GenerateCustomerPut()
        {
            Customer customer = new Customer
            {
                FirstName = "slenish",
                LastName = "moroius"
            };

            return JsonConvert.SerializeObject(customer);

        }
    }
}