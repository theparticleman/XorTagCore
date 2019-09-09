using System.Net;
using NUnit.Framework;
using RestSharp;

namespace XorTag.AcceptanceTests
{
    public class MapTests
    {
        public class When_getting_map
        {
            private IRestResponse mapResponse;

            [OneTimeSetUp]
            public void SetUp()
            {
                var settings = new AcceptanceTestSettings();
                var client = new RestClient(settings.BaseUrl);

                mapResponse = client.Execute(new RestRequest("map"));
            }

            [Test]
            public void It_should_succeed() => Assert.That(mapResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }    
}