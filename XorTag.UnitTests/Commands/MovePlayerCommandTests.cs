using System.Collections.Generic;
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
            private int playerStartY = 12;
            private Player player;
            private List<Player> allPlayers;

            [OneTimeSetUp]
            public void SetUp()
            {
                player = new Player { Id = 1234, Y = playerStartY };
                allPlayers = new List<Player> { player };
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
                result = ClassUnderTest.Execute("up", 1234);
            }

            [Test]
            public void It_should_move_the_player_up() => Assert.That(result.Y, Is.EqualTo(playerStartY - 1));
        }


    }
}