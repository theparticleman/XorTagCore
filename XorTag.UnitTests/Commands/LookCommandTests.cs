using XorTag.Commands;

namespace XorTag.UnitTests.Commands;

public class LookCommandTests
{
  public class When_executing_look_command_with_valid_player_id : WithAnAutomocked<LookCommand>
  {
    private CommandResult result;
    private Player player = new()
    {
      Id = 1234,
      X = 42,
      Y = 23,
      IsIt = true,
      Name = "Name 1",
    };

    [OneTimeSetUp]
    public void SetUp()
    {
      GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { player });
      GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(50);
      GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(30);
      result = ClassUnderTest.Execute(player.Id);
    }

    [Test]
    public void It_should_populate_all_the_easy_fields()
    {
      Assert.Multiple(() =>
      {
        Assert.That(result.Id, Is.EqualTo(player.Id));
        Assert.That(result.X, Is.EqualTo(player.X));
        Assert.That(result.Y, Is.EqualTo(player.Y));
        Assert.That(result.IsIt, Is.EqualTo(player.IsIt));
        Assert.That(result.MapWidth, Is.EqualTo(50));
        Assert.That(result.MapHeight, Is.EqualTo(30));
        Assert.That(result.Name, Is.EqualTo(player.Name));
        Assert.That(result.Players, Is.Not.Null);
      });
    }
  }
}