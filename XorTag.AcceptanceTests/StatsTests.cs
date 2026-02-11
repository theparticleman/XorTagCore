namespace XorTag.AcceptanceTests;

public class When_one_player_is_registered
{
    private ApiResponse registerResponse;
    private HttpResponseMessage statsResponse;
    private StatsResponse statsResponseData;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        var factory = TestHelpers.CreateTestFactory();
        var client = factory.CreateClient();

        await client.GetAsync("/admin/clearall");

        var response = await client.GetAsync("/register");
        registerResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        statsResponse = await client.GetAsync("/stats");
        statsResponseData = await statsResponse.Content.ReadFromJsonAsync<StatsResponse>();
    }

    [Test]
    public void It_should_succeed() => Assert.That(statsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    [Test]
    public void It_should_return_is_it_player_name()
        => Assert.That(statsResponseData.IsItPlayerName, Is.EqualTo(registerResponse.Name));

    [Test]
    public void It_should_leave_winning_player_name_null() => Assert.That(statsResponseData.WinningPlayerName, Is.Null);
}
