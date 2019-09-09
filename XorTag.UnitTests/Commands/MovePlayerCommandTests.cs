using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using XorTag.Commands;
using XorTag.Domain;

namespace XorTag.UnitTests.Commands
{
    public class MovePlayerCommandTests
    {
        public class When_moving_player_up : WithAnAutomocked<MovePlayerCommand>
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
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
            }

            [TestCase("up", playerStartX, playerStartY - 1)]
            [TestCase("down", playerStartX, playerStartY + 1)]
            [TestCase("left", playerStartX - 1, playerStartY)]
            [TestCase("right", playerStartX + 1, playerStartY)]
            public void It_should_save_and_return_the_new_player_position(string direction, int expectedX, int expectedY)
            {
                result = ClassUnderTest.Execute(direction, 1234);
                Assert.That(result.X, Is.EqualTo(expectedX));
                Assert.That(result.Y, Is.EqualTo(expectedY));
                GetMock<IPlayerRepository>().Verify(x => x.UpdatePlayerPosition(It.Is<Player>(p => p.X == expectedX && p.Y == expectedY)));
            }
        }

    }
}