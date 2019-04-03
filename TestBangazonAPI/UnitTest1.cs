using System;
using System.Threading.Tasks;
using Xunit;

namespace TestBangazonAPI
{
    public class ProductCRUD
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
        public async Task Test_Insert_A_Student()
        {
            using (var client = new APIClientProvider().Client)
            {

                Product Allison = new Product
                {
                    FirstName = "Allison",
                    LastName = "Collins",
                    SlackHandle = "alpal26",
                    CohortId = 3
                };


                var AllisonAsJSON = JsonConvert.SerializeObject(Allison);

                var response = await client.PostAsync(
                    "/api/student",
                    new StringContent(AllisonAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();

                var newAllison = JsonConvert.DeserializeObject<Student>(responseBody);


                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Allison", newAllison.FirstName);
                Assert.Equal("Collins", newAllison.LastName);
            }
        }

        [Fact]
        public async Task Test_Modify_Student()
        {
            // New last name to change to and test
            string newLastName = "SoFly";

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                */
                Product modifiedAllison = new Product
                {
                    FirstName = "Allison",
                    LastName = newLastName,
                    SlackHandle = "alpal26",
                    CohortId = 3
                };
                var modifiedJAllisonAsJSON = JsonConvert.SerializeObject(modifiedAllison);

                var response = await client.PutAsync(
                    "/api/student/9",
                    new StringContent(modifiedJAllisonAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


                /*
                    GET section
                    Verify that the PUT operation was successful
                */
                var getAllison = await client.GetAsync("/api/product/9");
                getAllison.EnsureSuccessStatusCode();

                string getAllisonBody = await getAllison.Content.ReadAsStringAsync();
                Product newAllison = JsonConvert.DeserializeObject<Product>(getAllisonBody);

                Assert.Equal(HttpStatusCode.OK, getAllison.StatusCode);
                Assert.Equal(newLastName, newAllison.LastName);
            }
        }
        [Fact]
        public async Task Test_Delete_Student()
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
