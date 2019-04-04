using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class ProductTest
    {
        [Fact]
        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/product");
                string responseBody = await response.Content.ReadAsStringAsync();
                var ProductList = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(ProductList.Count > 0);
            }

        }

        [Fact]
        public async Task Test_Get_A_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/product/1");
                string responseBody = await response.Content.ReadAsStringAsync();
                var Product = JsonConvert.DeserializeObject<Product>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(Product.Id == 1);
            }

        }

        [Fact]
        public async Task Test_Insert_A_Product()
        {
            using (var client = new APIClientProvider().Client)
            {

                Product Bison = new Product
                {
                    ProductTypeId = 4,
                    CustomerId = 2,
                    Price = 15.99,
                    Title = "Bison Burger",
                    Description = "Lean Meat to eat",
                    Quantity = 3
                };


                var BisonAsJSON = JsonConvert.SerializeObject(Bison);

                var response = await client.PostAsync(
                    "/api/student",
                    new StringContent(BisonAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newBison = JsonConvert.DeserializeObject<Product>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Bison Burger", newBison.Title);
                Assert.Equal("Lean Meat to eat", newBison.Description);
            }
        }

        [Fact]
        public async Task Test_Edit_Product()
        {
            // New last name to change to and test
            string newDescription = "Butcher's Choice";

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                Product modifiedBison = new Product
                {
                    ProductTypeId = 4,
                    CustomerId = 2,
                    Price = 15.99,
                    Title = "Bison Burger",
                    Description = newDescription,
                    Quantity = 3
                };
                var modifiedBisonAsJSON = JsonConvert.SerializeObject(modifiedBison);

                var response = await client.PutAsync(
                    "/api/product/9",
                    new StringContent(modifiedBisonAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


                /*
                    GET section
                    Verify that the PUT operation was successful
                */
                var getBison = await client.GetAsync("/api/product/9");
                getBison.EnsureSuccessStatusCode();

                string getBisonBody = await getBison.Content.ReadAsStringAsync();
                Product newBison = JsonConvert.DeserializeObject<Product>(getBisonBody);

                Assert.Equal(HttpStatusCode.OK, getBison.StatusCode);
                Assert.Equal(newDescription, newBison.Description);
            }
        }
        [Fact]
        public async Task Test_Delete_Product()
        {

            using (var client = new APIClientProvider().Client)
            {

                var response = await client.DeleteAsync("/api/product/9");


                string responseBody = await response.Content.ReadAsStringAsync();
                var Product = JsonConvert.DeserializeObject<Product>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            }

        }
    }
}
