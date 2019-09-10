using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using XorTag.Commands;
using XorTag.Domain;

namespace XorTag.UnitTests.Commands
{
    public class StatsCommandTests
    {
        public class When_no_players_are_registered : WithAnAutomocked<StatsCommand>
        {
            private StatsResult result;

            [OneTimeSetUp]
            public void SetUp()
            {
                result = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_set_winning_player_to_null() => Assert.That(result.WinningPlayerName, Is.Null);

            [Test]
            public void It_should_set_is_it_player_to_null() => Assert.That(result.IsItPlayerName, Is.Null);

            [Test]
            public void It_should_have_no_players() => Assert.That(result.Players, Is.Empty);
        }

        public class When_one_player_is_registered : WithAnAutomocked<StatsCommand>
        {
            private StatsResult result;
            private Player player;

            [OneTimeSetUp]
            public void SetUp()
            {
                player = new Player { Name = "Gandalf", IsIt = true };
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { player });
                result = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_set_is_it_player_name() => Assert.That(result.IsItPlayerName, Is.EqualTo(player.Name));
        }

        public class When_two_players_are_registered : WithAnAutomocked<StatsCommand>
        {
            private StatsResult result;
            private Player notItPlayer;

            [OneTimeSetUp]
            public void SetUp()
            {
                var isItPlayer = new Player { Name = "Gandalf", IsIt = true };
                notItPlayer = new Player { Name = "Gimli", IsIt = false };
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { isItPlayer, notItPlayer });
                result = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_set_winning_player_name() => Assert.That(result.WinningPlayerName, Is.EqualTo(notItPlayer.Name));

            [Test]
            public void It_should_put_one_not_it_player_in_players_list()
                => Assert.That(result.Players.Select(x => x.Name), Is.EquivalentTo(new string[] { notItPlayer.Name }));
        }

        public class When_three_players_are_registered : WithAnAutomocked<StatsCommand>
        {
            private StatsResult result;
            private Player firstNotItPlayer;
            private Player secondNotItPlayer;

            [OneTimeSetUp]
            public void SetUp()
            {
                var isItPlayer = new Player { Name = "Gandalf", IsIt = true };
                firstNotItPlayer = new Player { Name = "Gimli", IsIt = false };
                secondNotItPlayer = new Player { Name = "Frodo", IsIt = false };
                GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(new List<Player> { isItPlayer, firstNotItPlayer, secondNotItPlayer });
                result = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_include_all_not_it_players_in_list()
                => Assert.That(result.Players.Select(x => x.Name), Is.EquivalentTo(new string[] { firstNotItPlayer.Name, secondNotItPlayer.Name }));
        }
    }
}