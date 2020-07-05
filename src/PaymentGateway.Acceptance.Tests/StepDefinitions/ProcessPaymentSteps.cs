namespace PaymentGateway.Acceptance.Tests.StepDefinitions
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using RestSharp;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using WriteModel.API.Models;

    [Binding]
    public class ProcessPaymentSteps
    {
        private PaymentInfo paymentInfo;
        private IRestResponse paymentResponse;

        [Given(@"'(.*)' is registered with LLoyds bank")]
        public void GivenIsRegisteredWithLLoydsBank(string merchantId)
        {
            //ScenarioContext.Current.Pending();
        }


        [Given(@"the following details about an order"), Scope(Feature="GetPaymentDetails")]
        public void GivenFollowingDetailsAboutAnOrder(Table table)
        {
            paymentInfo = table.CreateInstance<PaymentInfo>();
        }

        [When(@"payment gateway processes a payment")]
        public async Task WhenPaymentGatewayProcessesAPayment()
        {
            var client = new RestClient("https://localhost:5009");
            var request = new RestRequest("/make-payment", Method.POST);

            paymentResponse = await client.PostAsync<IRestResponse>(request);
        }

        [Then(@"an accepted result is returned with a payment id")]
        public void ThenAcceptedResultIsReturnedWithPaymentId()
        {
            paymentResponse.Should().NotBe(null);
            paymentResponse.IsSuccessful.Should().BeTrue();
            paymentResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);
           // var paymentId = paymentResponse.Content
        }
        
    }
}