using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using XorTag.Commands;
using XorTag.Domain;

namespace XorTag.UnitTests.Commands
{
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

            [SetUp]
            public void SetUp()
            {
                player = new Player { Id = 1234, X = playerStartX, Y = playerStartY };
                allPlayers = new List<Player> { player };
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            }

            [TestCase("up", playerStartX, playerStartY - 1)]
            [TestCase("down", playerStartX, playerStartY + 1)]
            [TestCase("left", playerStartX - 1, playerStartY)]
            [TestCase("right", playerStartX + 1, playerStartY)]
            public void It_should_save_and_return_the_new_player_position(string direction, int expectedX, int expectedY)
            {
                result = ClassUnderTest.Execute(direction, 1234);

                GetMock<IPlayerRepository>().Verify(x => x.UpdatePlayerPosition(It.Is<Player>(p => p.X == expectedX && p.Y == expectedY)));
                Assert.That(result.X, Is.EqualTo(expectedX));
                Assert.That(result.Y, Is.EqualTo(expectedY));
            }
        }

        public class When_moving_player_is_it : WithAnAutomocked<MovePlayerCommand>
        {
            private Player player;
            private CommandResult result;

            [SetUp]
            public void SetUp()
            {
                player = new Player { Id = 1234, X = 0, Y = 0, IsIt = true };
                var allPlayers = new List<Player> { player };
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
                result = ClassUnderTest.Execute("right", 1234);
            }

            [Test]
            public void It_should_keep_player_as_it() => Assert.That(result.IsIt, Is.True);
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
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);

                var result = ClassUnderTest.Execute(direction, 1234);

                Assert.That(result.X, Is.EqualTo(startX));
                Assert.That(result.Y, Is.EqualTo(startY));
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
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { movingPlayer, stationaryPlayer });
                result = ClassUnderTest.Execute("right", 1234);
            }

            [Test]
            public void It_should_not_move_player() => Assert.That(result.X, Is.EqualTo(23));
        }

        public class When_moving_to_an_occupied_space_and_moving_player_is_it : WithAnAutomocked<MovePlayerCommand>
        {
            private Player movingPlayer;
            private Player stationaryPlayer;
            private CommandResult result;

            [OneTimeSetUp]
            public void SetUp()
            {
                movingPlayer = new Player { Id = 1234, X = 23, Y = 12, IsIt = true };
                stationaryPlayer = new Player { Id = 2345, X = 24, Y = 12, IsIt = false };
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { movingPlayer, stationaryPlayer });
                result = ClassUnderTest.Execute("right", 1234);
            }

            [Test]
            public void It_should_set_moving_player_as_NOT_it()
            {
                Assert.That(result.IsIt, Is.False);
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

            [OneTimeSetUp]
            public void SetUp()
            {
                movingPlayer = new Player { Id = 1234, X = 23, Y = 12, IsIt = false };
                stationaryPlayer = new Player { Id = 2345, X = 24, Y = 12, IsIt = true };
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { movingPlayer, stationaryPlayer });
                result = ClassUnderTest.Execute("right", 1234);
            }

            [Test]
            public void It_should_set_moving_player_as_it()
            {
                Assert.That(result.IsIt, Is.True);
                GetMock<IPlayerRepository>().Verify(x => x.SavePlayerAsIt(1234));
            }

            [Test]
            public void It_should_set_stationary_player_as_NOT_it() => GetMock<IPlayerRepository>().Verify(x => x.SavePlayerAsNotIt(2345));
        }

    }
}