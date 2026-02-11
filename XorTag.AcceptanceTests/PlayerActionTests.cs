namespace XorTag.AcceptanceTests;

public class PlayerActionTests
{
    public class When_registering_a_new_player
    {
        private HttpResponseMessage response;
        private ApiResponse responseData;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            response = await client.GetAsync("/register");
            responseData = await response.Content.ReadFromJsonAsync<ApiResponse>();
        }

        [Test]
        public void It_should_succeed() => Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_assign_a_name() => Assert.That(responseData.Name, Is.Not.Empty);

        [Test]
        public void It_should_assign_an_id() => Assert.That(responseData.Id, Is.GreaterThan(0));
    }

    public class When_registering_multiple_players
    {
        private ApiResponse firstRegistrationResponse;
        private ApiResponse secondRegistrationResponse;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            var clearResponse = await client.GetAsync("/admin/clearall");
            Assert.That(clearResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var firstResponse = await client.GetAsync("/register");
            firstRegistrationResponse = await firstResponse.Content.ReadFromJsonAsync<ApiResponse>();

            var secondResponse = await client.GetAsync("/register");
            secondRegistrationResponse = await secondResponse.Content.ReadFromJsonAsync<ApiResponse>();
        }

        [Test]
        public void It_should_set_first_player_as_it() => Assert.That(firstRegistrationResponse.IsIt, Is.True);

        [Test]
        public void It_should_set_second_player_as_NOT_it() => Assert.That(secondRegistrationResponse.IsIt, Is.False);
    }

    public class When_moving_a_player
    {
        private ApiResponse registerResponse;
        private HttpResponseMessage moveResponse;
        private ApiResponse moveResponseData;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            do
            {
                var response = await client.GetAsync("/register");
                registerResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            } while (registerResponse.Y <= 0);

            await Task.Delay(1000);
            moveResponse = await client.GetAsync("/moveup/" + registerResponse.Id);
            moveResponseData = await moveResponse.Content.ReadFromJsonAsync<ApiResponse>();
        }

        [Test]
        public void It_should_succeed() => Assert.That(moveResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_not_use_a_player_at_the_top_of_the_map() => Assert.That(registerResponse.Y, Is.GreaterThan(0));

        [Test]
        public void It_should_move_the_player_up() => Assert.That(moveResponseData.Y, Is.EqualTo(registerResponse.Y - 1));
    }

    public class When_moving_player_with_invalid_id
    {
        [Test]
        public async Task It_should_result_in_404()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            var moveResponse = await client.GetAsync("/moveup/9999");

            Assert.That(moveResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    public class When_moving_player_in_invalid_direction
    {
        [Test]
        public async Task It_should_result_in_404()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            var response = await client.GetAsync("/register");
            var registerResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            await Task.Delay(1000);

            var moveResponse = await client.GetAsync("/moveinvalid/" + registerResponse.Id);

            Assert.That(moveResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    public class When_having_a_player_look
    {
        private ApiResponse registerResponse;
        private HttpResponseMessage lookResponse;
        private ApiResponse lookResponseData;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var factory = TestHelpers.CreateTestFactory();
            var client = factory.CreateClient();

            var response = await client.GetAsync("/register");
            registerResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            await Task.Delay(1000);
            lookResponse = await client.GetAsync("/look/" + registerResponse.Id);
            lookResponseData = await lookResponse.Content.ReadFromJsonAsync<ApiResponse>();
        }

        [Test]
        public void It_should_succeed() => Assert.That(lookResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        [Test]
        public void It_should_not_move_the_player()
        {
            Assert.That(registerResponse.X, Is.EqualTo(lookResponseData.X));
            Assert.That(registerResponse.Y, Is.EqualTo(lookResponseData.Y));
        }
    }
}
