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
    readonly CommandResult builtCommandResult = new();

    [OneTimeSetUp]
    public void SetUp()
    {
      var allPlayers = new List<Player> { player };
      GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
      GetMock<ICommandResultBuilder>()
        .Setup(x => x.Build(player, allPlayers))
        .Returns(builtCommandResult);
      result = ClassUnderTest.Execute(player.Id);
    }

    [Test]
    public void It_should_return_the_result_from_the_builder() => Assert.That(result, Is.EqualTo(builtCommandResult));

    [Test]
    public void It_should_update_last_active_time() => GetMock<IPlayerRepository>().Verify(x => x.UpdateLastActiveTime(player.Id));

    [Test]
    public void It_should_check_action_frequency() => GetMock<IActionFrequencyChecker>().Verify(x => x.CheckFreqency(player.Id));
  }

  public class When_executing_look_command_with_invalid_player_id : WithAnAutomocked<LookCommand>
  {
    [Test]
    public void It_should_throw_not_found_exception()
    {
      var invalidPlayerId = -1;
      var existingPlayers = new List<Player> { new() { Id = 1 }, new() { Id = 2 } };
      GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(existingPlayers);

      Assert.Throws<NotFoundException>(() => ClassUnderTest.Execute(invalidPlayerId));
    }
  }
}