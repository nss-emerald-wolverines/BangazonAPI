using BangazonAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
// using Microsoft.AspNetCore.Mvc.Testing;


namespace TestBangazonAPI
{
    public class OrderTest
    {
        [Fact]
        public async Task Get_All_Orders_Returns_Some_Orders()
        {
            using (HttpClient client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Order");

                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Order/1");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                int customerId = order.CustomerId;
                Assert.Equal(1, actual: customerId);
            }
        }

        [Fact]
        public async Task Test_Get_Imaginary_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/order/9999999");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Create_And_Delete_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                Order mark = new Order
                {
                    CustomerId = 2,
                    PaymentTypeId = 4
                };
                var markAsJSON = JsonConvert.SerializeObject(mark);

                var response = await client.PostAsync(
                    "/api/Order",
                    new StringContent(markAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var newMark = JsonConvert.DeserializeObject<Order>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(2, newMark.CustomerId);
                Assert.Equal(4, newMark.PaymentTypeId);

                // DELETE

                var deleteResponse = await client.DeleteAsync($"/api/order/{newMark.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Order_To_Make_PaymentType_Null()
        {           
            int noPymt = 0;

            using (var client = new APIClientProvider().Client)
            {
                Order changedOrder= new Order
                    {
                        CustomerId = 1,
                        PaymentTypeId = noPymt
                    };
                    var changedOrderAsJSON = JsonConvert.SerializeObject(changedOrder);

                    var response = await client.PutAsync("/api/order/1",
                        new StringContent(changedOrderAsJSON, Encoding.UTF8, "application/json")
                    );
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                    var getChangedOrder = await client.GetAsync("/api/order/1");
                    getChangedOrder.EnsureSuccessStatusCode();

                    string getTheBody = await getChangedOrder.Content.ReadAsStringAsync();
                    Order newOrder = JsonConvert.DeserializeObject<Order>(getTheBody);

                    Assert.Equal(HttpStatusCode.OK, getChangedOrder.StatusCode);
                    Assert.Equal(noPymt, newOrder.PaymentTypeId);
                }
            }

        [Fact]
        public async Task Test_Get_All_Orders_With_OrderProducts()
        {
            // ARRANGE
            using (var client = new APIClientProvider().Client)
            {
                // ACT
                var response = await client.GetAsync("/api/order");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var orderProductList = JsonConvert.DeserializeObject<List<Order>>(responseBody);

                // ASSERT
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orderProductList.Count > 0);
            }
        }

    }
    }
