using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace XorTag.UnitTests.Domain;

public class PlayerInactivityCheckerTests
{
  [Test]
  public async Task It_should_remove_inactive_player()
  {
    var loggerMock = new Mock<ILogger<PlayerInactivityChecker>>();
    var settingsMock = new Mock<ISettings>();
    settingsMock.Setup(x => x.InactivityTimeoutMilliseconds).Returns(10);

    var playerRepository = new InMemoryPlayerRepository();
    var classUnderTest = new PlayerInactivityChecker(playerRepository, settingsMock.Object, loggerMock.Object);

    await classUnderTest.StartAsync(CancellationToken.None);
    var inactivePlayer = new Player
    {
      Id = 1234,
    };
    var activePlayer = new Player
    {
      Id = 1235,
    };
    playerRepository.Save(inactivePlayer);
    playerRepository.Save(activePlayer);
    playerRepository.UpdateLastActiveTime(inactivePlayer.Id);
    Assert.That(playerRepository.GetAllPlayers(), Has.Count.EqualTo(2));
    Thread.Sleep(25);
    Assert.That(playerRepository.GetAllPlayers(), Has.Count.EqualTo(1));
    await classUnderTest.StopAsync(CancellationToken.None);
  }
}