using XorTag.Commands;

namespace XorTag.UnitTests.Commands;

public class MovePlayerCommandTests
{
    private const int mapHeight = 30;
    private const int mapWidth = 50;

    public class When_moving_player : WithAnAutomocked<MovePlayerCommand>
    {
        private CommandResult result;
        private const int playerStartY = 12;
        private const int playerStartX = 23;
        private Player player;
        private List<Player> allPlayers;
        private CommandResult builtCommandResult = new CommandResult();
        private Player capturedPlayer;

        [SetUp]
        public void SetUp()
        {
            player = new Player { Id = 1234, X = playerStartX, Y = playerStartY, Name = "player name" };
            allPlayers = new List<Player> { player };
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            GetMock<ICommandResultBuilder>()
                .Setup(x => x.Build(It.IsAny<Player>(), It.IsAny<List<Player>>()))
                .Callback<Player, List<Player>>((p, _) => capturedPlayer = p)
                .Returns(builtCommandResult);
        }

        [TestCase("up", playerStartX, playerStartY - 1)]
        [TestCase("Up", playerStartX, playerStartY - 1)]
        [TestCase("down", playerStartX, playerStartY + 1)]
        [TestCase("Down", playerStartX, playerStartY + 1)]
        [TestCase("left", playerStartX - 1, playerStartY)]
        [TestCase("right", playerStartX + 1, playerStartY)]
        public void It_should_save_and_return_the_new_player_position(string direction, int expectedX, int expectedY)
        {
            result = ClassUnderTest.Execute(direction, 1234);

            GetMock<IPlayerRepository>().Verify(x => x.UpdatePlayerPosition(It.Is<Player>(p => p.X == expectedX && p.Y == expectedY)));

            Assert.That(capturedPlayer.X, Is.EqualTo(expectedX));
            Assert.That(capturedPlayer.Y, Is.EqualTo(expectedY));
            Assert.That(result, Is.EqualTo(builtCommandResult));
        }

        [Test]
        public void It_should_update_the_last_active_time() => GetMock<IPlayerRepository>().Verify(x => x.UpdateLastActiveTime(player.Id));
    }

    public class When_moving_player_is_it : WithAnAutomocked<MovePlayerCommand>
    {
        private Player player;
        private CommandResult result;
        private Player capturedPlayer;
        private CommandResult builtCommandResult = new CommandResult();

        [SetUp]
        public void SetUp()
        {
            player = new Player { Id = 1234, X = 0, Y = 0, IsIt = true };
            var allPlayers = new List<Player> { player };
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            GetMock<ICommandResultBuilder>()
                .Setup(x => x.Build(IsAny<Player>(), allPlayers))
                .Callback<Player, List<Player>>((p, _) => capturedPlayer = p)
                .Returns(builtCommandResult);
            result = ClassUnderTest.Execute("right", 1234);
        }

        [Test]
        public void It_should_keep_player_as_it() => Assert.That(capturedPlayer.IsIt, Is.True);

        [Test]
        public void It_should_return_the_result_from_the_builder() => Assert.That(result, Is.EqualTo(builtCommandResult));
    }

    public class When_moving_near_map_edges : WithAnAutomocked<MovePlayerCommand>
    {
        [TestCase("up", 23, 0)]
        [TestCase("down", 23, mapHeight - 1)]
        [TestCase("left", 0, 12)]
        [TestCase("right", mapWidth - 1, 12)]
        public void It_should_not_let_player_move_off_the_map(string direction, int startX, int startY)
        {
            var player = new Player { Id = 1234, X = startX, Y = startY };
            var allPlayers = new List<Player> { player };
            var builtCommandResult = new CommandResult();
            Player capturedPlayer = null;
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            GetMock<ICommandResultBuilder>()
                .Setup(x => x.Build(IsAny<Player>(), IsAny<List<Player>>()))
                .Callback<Player, List<Player>>((p, _) => capturedPlayer = p)
                .Returns(builtCommandResult);

            var result = ClassUnderTest.Execute(direction, 1234);

            Assert.That(capturedPlayer.X, Is.EqualTo(startX));
            Assert.That(capturedPlayer.Y, Is.EqualTo(startY));
            Assert.That(capturedPlayer.Id, Is.EqualTo(player.Id));
            Assert.That(result, Is.EqualTo(builtCommandResult));
        }
    }

    public class When_moving_to_an_occupied_space_and_neither_player_is_it : WithAnAutomocked<MovePlayerCommand>
    {
        private Player movingPlayer;
        private Player stationaryPlayer;
        private CommandResult result;

        [OneTimeSetUp]
        public void SetUp()
        {
            movingPlayer = new Player { Id = 1234, X = 23, Y = 12, IsIt = false };
            stationaryPlayer = new Player { Id = 2345, X = 24, Y = 12, IsIt = false };
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { movingPlayer, stationaryPlayer });
            result = ClassUnderTest.Execute("right", 1234);
        }

        [Test]
        public void It_should_not_move_player()
        {
            GetMock<ICommandResultBuilder>().Verify(x => x.Build(It.Is<Player>(p => p.X == movingPlayer.X && p.Y == movingPlayer.Y), IsAny<List<Player>>()));
        }
    }

    public class When_moving_to_an_occupied_space_and_moving_player_is_it : WithAnAutomocked<MovePlayerCommand>
    {
        private Player movingPlayer;
        private Player stationaryPlayer;
        private CommandResult result;
        private Player capturedPlayer;

        [OneTimeSetUp]
        public void SetUp()
        {
            movingPlayer = new Player { Id = 1234, X = 23, Y = 12, IsIt = true };
            stationaryPlayer = new Player { Id = 2345, X = 24, Y = 12, IsIt = false };
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            var allPlayers = new List<Player> { movingPlayer, stationaryPlayer };
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            GetMock<ICommandResultBuilder>()
                .Setup(x => x.Build(IsAny<Player>(), allPlayers))
                .Callback<Player, List<Player>>((p, _) => capturedPlayer = p);

            result = ClassUnderTest.Execute("right", 1234);
        }

        [Test]
        public void It_should_set_moving_player_as_NOT_it()
        {
            Assert.That(capturedPlayer, Is.Not.Null);
            Assert.That(capturedPlayer.IsIt, Is.False);
            GetMock<IPlayerRepository>().Verify(x => x.SavePlayerAsNotIt(1234));
        }

        [Test]
        public void It_should_set_stationary_player_as_it() => GetMock<IPlayerRepository>().Verify(x => x.SavePlayerAsIt(2345));
    }

    public class When_moving_to_an_occupied_space_and_stationary_player_is_it : WithAnAutomocked<MovePlayerCommand>
    {
        private Player movingPlayer;
        private Player stationaryPlayer;
        private CommandResult result;
        private CommandResult builtCommandResult = new CommandResult();
        private Player capturedPlayer;

        [OneTimeSetUp]
        public void SetUp()
        {
            movingPlayer = new Player { Id = 1234, X = 23, Y = 12, IsIt = false };
            stationaryPlayer = new Player { Id = 2345, X = 24, Y = 12, IsIt = true };
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            var allPlayers = new List<Player> { movingPlayer, stationaryPlayer };
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            GetMock<ICommandResultBuilder>()
                .Setup(x => x.Build(IsAny<Player>(), allPlayers))
                .Callback<Player, List<Player>>((p, _) => capturedPlayer = p)
                .Returns(builtCommandResult);
            result = ClassUnderTest.Execute("right", 1234);
        }

        [Test]
        public void It_should_set_moving_player_as_it()
        {
            Assert.That(capturedPlayer.IsIt, Is.True);
            GetMock<IPlayerRepository>().Verify(x => x.SavePlayerAsIt(1234));
        }

        [Test]
        public void It_should_set_stationary_player_as_NOT_it() => GetMock<IPlayerRepository>().Verify(x => x.SavePlayerAsNotIt(2345));

        [Test]
        public void It_should_return_the_result_from_the_builder()
        {
            Assert.That(result, Is.EqualTo(builtCommandResult));
        }
    }

    public class When_moving_in_invalid_direction : WithAnAutomocked<MovePlayerCommand>
    {
        [Test]
        public void It_should_throw_exception()
        {
            var player = new Player { Id = 1234 };
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { player });
            Assert.Throws<NotFoundException>(() => ClassUnderTest.Execute("invalid-direction", 1234));
        }
    }

    public class When_moving_with_invalid_id : WithAnAutomocked<MovePlayerCommand>
    {
        [Test]
        public void It_should_throw_exception()
        {
            var player = new Player { Id = 1234 };
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { player });
            Assert.Throws<NotFoundException>(() => ClassUnderTest.Execute("up", 9999));
        }
    }

}