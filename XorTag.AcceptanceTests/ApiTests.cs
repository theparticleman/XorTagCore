using System.Net;
using NUnit.Framework;
using RestSharp;

namespace XorTag.AcceptanceTests
{
    public class ApiTests
    {
        public class When_registering_a_new_player
        {
            private IRestResponse<ApiResult> response;

            [OneTimeSetUp]
            public void SetUp()
            {
                var settings = new AcceptanceTestSettings();
                var client = new RestClient(settings.BaseUrl);
                var request = new RestRequest("register");

                response = client.Execute<ApiResult>(request);
            }

            [Test]
            public void It_should_succeed() => Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            [Test]
            public void It_should_set_first_player_as_it() => Assert.That(response.Data.IsIt, Is.True);

            [Test]
            public void It_should_assign_a_name() => Assert.That(response.Data.Name, Is.Not.Empty);

            [Test]
            public void It_should_assign_an_id() => Assert.That(response.Data.Id, Is.GreaterThan(0));
        }

        public class When_moving_a_player
        {
            private IRestResponse<ApiResult> registerResponse;
            private IRestResponse<ApiResult> moveReponse;

            [OneTimeSetUp]
            public void SetUp()
            {
                var settings = new AcceptanceTestSettings();
                var client = new RestClient(settings.BaseUrl);

                do //make sure that the player we've registered isn't at the top of the map
                {
                    registerResponse = client.Execute<ApiResult>(new RestRequest("register"));
                } while (registerResponse.Data.Y <= 0);
                moveReponse = client.Execute<ApiResult>(new RestRequest("/moveup/" + registerResponse.Data.Id));
            }

            [Test]
            public void It_should_succeed() => Assert.That(moveReponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            [Test]
            public void It_should_not_use_a_player_at_the_top_of_the_map() => Assert.That(registerResponse.Data.Y, Is.GreaterThan(0));

            [Test]
            public void It_should_move_the_player_up() => Assert.That(moveReponse.Data.Y, Is.EqualTo(registerResponse.Data.Y - 1));
        }
    }
}