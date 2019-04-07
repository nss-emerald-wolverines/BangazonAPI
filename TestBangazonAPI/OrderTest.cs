using BangazonAPI.Models;
using Newtonsoft.Json;
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
        public async Task Test_Get_All_Orders_With_ProductsTwo()
        {
            // ARRANGE
            using (var client = new APIClientProvider().Client)
            {
                // ACT
                var response = await client.GetAsync("/api/order");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var productList = JsonConvert.DeserializeObject<List<Order>>(responseBody);

                // ASSERT
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(condition: productList.Count > 0);
            }
        }

        [Fact]
        public async Task Get_Specific_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/api/order/1");
                string responseBody = await response.Content.ReadAsStringAsync();
                var Order= JsonConvert.DeserializeObject<Order>(responseBody);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(condition: Order.OrderId == 1);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                var orderGetInitialResponse = await client.GetAsync("api/orders");
                string initialResponseBody = await orderGetInitialResponse.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<Order>>(initialResponseBody);
                Assert.Equal(HttpStatusCode.OK, orderGetInitialResponse.StatusCode);
                var orderTypeObject = orderList[0];

                //BEGIN GET SPECIFIC TESTING
                var response = await client.GetAsync($"api/orders/{orderTypeObject.OrderId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                var orderReturned = JsonConvert.DeserializeObject<Order>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orderReturned.OrderId == orderTypeObject.OrderId);
            }
        }

        [Fact]
        public async Task Test_Create()
        {
            using (var client = new APIClientProvider().Client)
            {
                Order mark = new Order
                {
                    CustomerId = 2,
                    PaymentTypeId = 1
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
            }
        }

        [Fact]
        public async Task Test_Modify_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                var orderGetInitialResponse = await client.GetAsync("api/orders");
                string initialResponseBody = await orderGetInitialResponse.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<Order>>(initialResponseBody);
                Assert.Equal(HttpStatusCode.OK, orderGetInitialResponse.StatusCode);

                var orderObject = orderList[0];
                var defaultCustomerId = orderObject.CustomerId;

                /* PUT section */
                orderObject.CustomerId = 1;

                var modifiedOrderAsJson = JsonConvert.SerializeObject(orderObject);
                var response = await client.PutAsync($"api/orders/{orderObject.OrderId}",
                    new StringContent(modifiedOrderAsJson, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getOrder = await client.GetAsync($"api/orders/{orderObject.OrderId}");
                getOrder.EnsureSuccessStatusCode();

                string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);
                Assert.Equal(1, newOrder.CustomerId);

                newOrder.CustomerId = defaultCustomerId;
                var returnOrderToDefault = JsonConvert.SerializeObject(newOrder);

                var putOrderToDefault = await client.PutAsync($"api/orders/{newOrder.OrderId}",
                    new StringContent(returnOrderToDefault, Encoding.UTF8, "application/json"));

                string originalOrderObject = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            }
        }


        [Fact]
        public async Task Test_Remove_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                var orderGetInitialResponse = await client.GetAsync("api/orders");
                string initialResponseBody = await orderGetInitialResponse.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<Order>>(initialResponseBody);
                Assert.Equal(HttpStatusCode.OK, orderGetInitialResponse.StatusCode);

                int removeLastObject = orderList.Count - 1;
                var orderObject = orderList[removeLastObject];

                var response = await client.DeleteAsync($"api/orders/{orderObject.OrderId}");
                string responseBody = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var getOrder = await client.GetAsync($"api/orders/{orderObject.OrderId}");
                getOrder.EnsureSuccessStatusCode();

                string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);

                Assert.Equal(HttpStatusCode.NoContent, getOrder.StatusCode);
            }

        }




    }
}

        /*  [Fact]
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
                }     */

        /*  [Fact]
              public async Task Test_Remove_Order()
              {
                  using (var client = new APIClientProvider().Client)
                  {
                      var orderGetInitialResponse = await client.GetAsync("api/orders");
                      string initialResponseBody = await orderGetInitialResponse.Content.ReadAsStringAsync();
                      var orderList = JsonConvert.DeserializeObject<List<Order>>(initialResponseBody);
                      Assert.Equal(HttpStatusCode.OK, orderGetInitialResponse.StatusCode);

                      int removeLastObject = orderList.Count - 1;
                      var orderObject = orderList[removeLastObject];

                      var response = await client.DeleteAsync($"api/orders/{orderObject.Id}");
                      string responseBody = await response.Content.ReadAsStringAsync();
                      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                      var getOrder = await client.GetAsync($"api/orders/{orderObject.Id}");
                      getOrder.EnsureSuccessStatusCode();

                      string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                      Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);

                      Assert.Equal(HttpStatusCode.NoContent, getOrder.StatusCode);
                  }

              } */

    /*    [Fact]
            public async Task Test_Get_NonExistent_Order()
            {
                using (var client = new APIClientProvider().Client)
                {
                    var response = await client.GetAsync("/api/order/9999999");
                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                }
            }   */