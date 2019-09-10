using NUnit.Framework;
using XorTag.Commands;

namespace XorTag.UnitTests.Commands
{
    public class StatsCommandTests
    {
        public class When_no_players_are_registered: WithAnAutomocked<StatsCommand>
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
    }
}