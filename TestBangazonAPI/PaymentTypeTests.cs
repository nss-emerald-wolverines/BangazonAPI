using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using BangazonAPI.Models;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

namespace TestBangazonAPI
{
  public class PaymentTypeTest
  {
    [Fact]
    public async Task Test_Get_All_PaymentTypes()
    {
      using (var client = new APIClientProvider().Client)
      {
        var response = await client.GetAsync("/api/PaymentType");
        string responseBody = await response.Content.ReadAsStringAsync();
        var ProductList = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(ProductList.Count > 0);
      }
    }
    [Fact]
    public async Task Test_Get_A_PaymentType()
    {
      using (var client = new APIClientProvider().Client)
      {
        var response = await client.GetAsync("/api/PaymentType/1");
        string responseBody = await response.Content.ReadAsStringAsync();
        var PaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(PaymentType.Id == 1);
      }
    }
    [Fact]
    public async Task Test_Post_A_PaymentType()
    {
      using (var client = new APIClientProvider().Client)
      {
        PaymentType TestPaymentType = new PaymentType
        {
          AcctNumber = 42012346,
          Name = "AMEX_Platinum",
          CustomerId = 1
        };
        var PaymentTypeAsJSON = JsonConvert.SerializeObject(TestPaymentType);

        var response = await client.PostAsync(
            "/api/PaymentType",
            new StringContent(PaymentTypeAsJSON, Encoding.UTF8, "application/json")
        );
        string responseBody = await response.Content.ReadAsStringAsync();

        var newPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(TestPaymentType.AcctNumber, newPaymentType.AcctNumber);
        Assert.Equal(TestPaymentType.Name, newPaymentType.Name);
        Assert.Equal(TestPaymentType.CustomerId, newPaymentType.CustomerId);
      }
    }
    [Fact]
    public async Task Test_Edit_PaymentType()
    {
      // New name change to test
      string newName = "AMEX_Red";

      using (var client = new APIClientProvider().Client)
      {
        /*
            PUT section
        */
        PaymentType modifiedPaymentType = new PaymentType
        {
          Id = 18,
          AcctNumber = 42012346,
          Name = newName,
          CustomerId = 1
        };
        var modifiedPaymentTypeAsJSON = JsonConvert.SerializeObject(modifiedPaymentType);

        var response = await client.PutAsync(
            "/api/PaymentType/18",
            new StringContent(modifiedPaymentTypeAsJSON, Encoding.UTF8, "application/json")
        );
        string responseBody = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


        /*
            GET section
            Verify that the PUT operation was successful
        */
        var getPaymentType = await client.GetAsync("/api/PaymentType/18");
        getPaymentType.EnsureSuccessStatusCode();

        string getPaymentTypeBody = await getPaymentType.Content.ReadAsStringAsync();
        PaymentType newPaymentType = JsonConvert.DeserializeObject<PaymentType>(getPaymentTypeBody);

        Assert.Equal(HttpStatusCode.OK, getPaymentType.StatusCode);
        Assert.Equal(newName, newPaymentType.Name);
      }
    }
    [Fact]
    public async Task Test_Delete_PaymentType()
    {

      using (var client = new APIClientProvider().Client)
      {

        var response = await client.DeleteAsync("/api/PaymentType/22");


        string responseBody = await response.Content.ReadAsStringAsync();
        var Product = JsonConvert.DeserializeObject<PaymentType>(responseBody);

        /*
            ASSERT
        */
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

      }
    }
  }
}

      