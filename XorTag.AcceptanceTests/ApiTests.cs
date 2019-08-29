using System.Net;
using NUnit.Framework;
using RestSharp;

namespace Tests
{
    public class ApiTests
    {
        public class When_registering_a_new_player
        {
            private IRestResponse response;

            [SetUp]
            public void SetUp()
            {
                var settings = new AcceptanceTestSettings();
                var client = new RestClient(settings.BaseUrl);
                var request = new RestRequest("register");

                response = client.Execute(request);
            }

            [Test]
            public void It_should_suceed() => Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}