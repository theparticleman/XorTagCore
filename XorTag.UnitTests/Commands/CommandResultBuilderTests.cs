using System.Linq;
using XorTag.Commands;

namespace XorTag.UnitTests.Commands;

public class CommandResultBuilderTests : WithAnAutomocked<CommandResultBuilder>
{
  const int mapWidth = 123;
  const int mapHeight = 234;
  Player player;
  Player playerInRange;
  Player playerOutOfRange;
  CommandResult result;

  [OneTimeSetUp]
  public void SetUp()
  {
    player = new Player
    {
      X = 42,
      Y = 23,
      Name = "player name",
      IsIt = true,
      Id = 1234,
    };

    playerInRange = new Player
    {
      X = 43,
      Y = 23,
      Name = "player in range",
      IsIt = false,
      Id = 1235,
    };

    playerOutOfRange = new Player
    {
      X = 52,
      Y = 23,
      Name = "player out of range",
      IsIt = false,
      Id = 1236,
    };

    GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
    GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);

    var allPlayers = new List<Player> { player, playerInRange };

    result = ClassUnderTest.Build(player, allPlayers);
  }

  [Test]
  public void It_should_populate_all_player_fields()
  {
    Assert.Multiple(() =>
    {
      Assert.That(result, Is.Not.Null);
      Assert.That(result.X, Is.EqualTo(player.X));
      Assert.That(result.Y, Is.EqualTo(player.Y));
      Assert.That(result.Id, Is.EqualTo(player.Id));
      Assert.That(result.Name, Is.EqualTo(player.Name));
      Assert.That(result.IsIt, Is.EqualTo(player.IsIt));
    });
  }

  [Test]
  public void It_should_set_map_fields()
  {
    Assert.Multiple(() => {
      Assert.That(result.MapHeight, Is.EqualTo(mapHeight));
      Assert.That(result.MapWidth, Is.EqualTo(mapWidth));
    });
  }

  [Test]
  public void It_should_populate_information_for_players_that_are_close_enough()
  {
    Assert.That(result.Players, Has.Count.EqualTo(1));
    var returnedPlayerInRange = result.Players.SingleOrDefault(p => p.X == playerInRange.X && p.Y == playerInRange.Y);
    Assert.That(returnedPlayerInRange, Is.Not.Null);
    Assert.That(returnedPlayerInRange.IsIt, Is.False);
  }

  [Test]
  public void It_should_NOT_populate_information_for_players_that_are_NOT_close_enough()
  {
    var returnedPlayerOutOfRange = result.Players.SingleOrDefault(p => p.X == playerOutOfRange.X && p.Y == playerOutOfRange.Y);
    Assert.That(returnedPlayerOutOfRange, Is.Null);
  }
}