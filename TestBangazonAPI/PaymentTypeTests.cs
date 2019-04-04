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
  public class PaymentTypeTests
  {
    [Fact]
    public async Task Test_Create_PaymentType()
    {
      /*
          Generate a new instance of an HttpClient that you can
          use to generate HTTP requests to your API controllers.
          The `using` keyword will automatically dispose of this
          instance of HttpClient once your code is done executing.
      */
      using (var client = new APIClientProvider().Client)
      {
        /*
            ARRANGE
        */

        // Construct a new PaymentType object to be sent to the API
        PaymentType PaymentTypeTest = new PaymentType
        {
          AcctNumber = 42012345,
          Name = "AMEX_Black",
          CustomerId = 1
        };

        // Serialize the C# object into a JSON string
        var paymentTypeAsJSON = JsonConvert.SerializeObject(PaymentTypeTest);

        /*
            ACT
        */

        // Use the client to send the request and store the response
        var response = await client.PostAsync(
            "/api/PaymentType",
            new StringContent(paymentTypeAsJSON, Encoding.UTF8, "application/json")
        );

        // Store the JSON body of the response
        string responseBody = await response.Content.ReadAsStringAsync();

        // Deserialize the JSON into an instance of Animal
        var newPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);


        /*
            ASSERT
        */

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(42012345, newPaymentType.AcctNumber);
        Assert.Equal("AMEX_Black", newPaymentType.Name);
        Assert.Equal(1, newPaymentType.CustomerId);
      }
    }
    [Fact]
    public async Task Test_Get_All_PaymentTypes()
    {

      using (var client = new APIClientProvider().Client)
      {
        /*
            ARRANGE
        */


        /*
            ACT
        */
        var response = await client.GetAsync("/api/animals");


        string responseBody = await response.Content.ReadAsStringAsync();
        var animalList = JsonConvert.DeserializeObject<List<Animal>>(responseBody);

        /*
            ASSERT
        */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(animalList.Count > 0);
      }
    }
  }
}
