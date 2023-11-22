namespace XorTag.AcceptanceTests;

public class When_one_player_is_registered
{
    private IRestResponse<ApiResponse> registerResponse;
    private IRestResponse<StatsResponse> statsResponse;

    [OneTimeSetUp]
    public void SetUp()
    {
        var settings = new AcceptanceTestSettings();
        var client = new RestClient(settings.BaseUrl);

        client.Execute(new RestRequest("admin/clearall"));
        registerResponse = client.Execute<ApiResponse>(new RestRequest("register"));

        statsResponse = client.Execute<StatsResponse>(new RestRequest("stats"));
    }

    [Test]
    public void It_should_succeed() => Assert.That(statsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    [Test]
    public void It_should_return_is_it_player_name()
        => Assert.That(statsResponse.Data.IsItPlayerName, Is.EqualTo(registerResponse.Data.Name));

    [Test]
    public void It_should_leave_winning_player_name_null() => Assert.That(statsResponse.Data.WinningPlayerName, Is.Null);
}