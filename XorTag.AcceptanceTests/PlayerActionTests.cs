namespace XorTag.AcceptanceTests;

public class PlayerActionTests
{
    public class When_registering_a_new_player
    {
        private IRestResponse<ApiResponse> response;

        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);
            var request = new RestRequest("register");

            response = client.Execute<ApiResponse>(request);
        }

        [Test]
        public void It_should_succeed() => Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_assign_a_name() => Assert.That(response.Data.Name, Is.Not.Empty);

        [Test]
        public void It_should_assign_an_id() => Assert.That(response.Data.Id, Is.GreaterThan(0));
    }

    public class When_registering_multiple_players
    {
        private IRestResponse<ApiResponse> firstRegistrationResponse;
        private IRestResponse<ApiResponse> secondRegistrationResponse;

        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);

            var clearResponse = client.Execute(new RestRequest("admin/clearall"));
            Assert.That(clearResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            firstRegistrationResponse = client.Execute<ApiResponse>(new RestRequest("register"));
            secondRegistrationResponse = client.Execute<ApiResponse>(new RestRequest("register"));
        }

        [Test]
        public void It_should_set_first_player_as_it() => Assert.That(firstRegistrationResponse.Data.IsIt, Is.True);

        [Test]
        public void It_should_set_second_player_as_NOT_it() => Assert.That(secondRegistrationResponse.Data.IsIt, Is.False);
    }

    public class When_moving_a_player
    {
        private IRestResponse<ApiResponse> registerResponse;
        private IRestResponse<ApiResponse> moveReponse;

        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);

            do //make sure that the player we've registered isn't at the top of the map
            {
                registerResponse = client.Execute<ApiResponse>(new RestRequest("register"));
            } while (registerResponse.Data.Y <= 0);
            moveReponse = client.Execute<ApiResponse>(new RestRequest("/moveup/" + registerResponse.Data.Id));
        }

        [Test]
        public void It_should_succeed() => Assert.That(moveReponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_not_use_a_player_at_the_top_of_the_map() => Assert.That(registerResponse.Data.Y, Is.GreaterThan(0));

        [Test]
        public void It_should_move_the_player_up() => Assert.That(moveReponse.Data.Y, Is.EqualTo(registerResponse.Data.Y - 1));
    }

    public class When_moving_player_with_invalid_id
    {
        [Test]
        public void It_should_result_in_404()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);

            var moveReponse = client.Execute<ApiResponse>(new RestRequest("/moveup/" + 9999));

            Assert.That(moveReponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    public class When_moving_player_in_invalid_direction
    {
        [Test]
        public void It_should_result_in_404()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);
            var registerResponse = client.Execute<ApiResponse>(new RestRequest("register"));

            var moveReponse = client.Execute<ApiResponse>(new RestRequest("/moveinvalid/" + registerResponse.Data.Id));

            Assert.That(moveReponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    public class When_having_a_player_look
    {
        private IRestResponse<ApiResponse> registerResponse;
        private IRestResponse<ApiResponse> lookResponse;

        [OneTimeSetUp]
        public void SetUp()
        {
            var settings = new AcceptanceTestSettings();
            var client = new RestClient(settings.BaseUrl);
            registerResponse = client.Execute<ApiResponse>(new RestRequest("register"));
            Assert.That(registerResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            lookResponse = client.Execute<ApiResponse>(new RestRequest("/look/" + registerResponse.Data.Id));
        }

        [Test]
        public void It_should_succeed() => Assert.That(lookResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_not_move_the_player()
        {
            Assert.That(registerResponse.Data.X, Is.EqualTo(lookResponse.Data.X));
            Assert.That(registerResponse.Data.Y, Is.EqualTo(lookResponse.Data.Y));
        }
    }

}