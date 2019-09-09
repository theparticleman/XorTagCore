using XorTag.Commands;
using NUnit.Framework;
using XorTag.Domain;
using System.Collections.Generic;

namespace XorTag.UnitTests.Commands
{
    public class RegisterPlayerCommandTests
    {
        public class When_registering_a_new_player: WithAnAutomocked<RegisterPlayerCommand>
        {
            private CommandResult result;
            private const string name = "generated-name";
            private const int mapWidth = 40;
            private const int mapHeight = 20;

            [OneTimeSetUp]
            public void SetUp()
            {
                GetMock<IIdGenerator>().Setup(x => x.GenerateId(IsAny<IEnumerable<int>>())).Returns(1234);
                GetMock<INameGenerator>().Setup(x => x.GenerateName(IsAny<IEnumerable<string>>())).Returns(name);
                var randomValue = 23;
                GetMock<IRandom>().Setup(x => x.Next(IsAny<int>())).Returns(() => randomValue++);
                GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
                GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
                result = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_generate_a_new_id() => Assert.That(result.Id, Is.EqualTo(1234));
            
            [Test]
            public void It_should_generate_a_name() => Assert.That(result.Name, Is.EqualTo(name));

            [Test]
            public void It_make_player_it() => Assert.That(result.IsIt, Is.True);

            [Test]
            public void It_should_set_map_dimensions() 
            {
                Assert.That(result.MapWidth, Is.EqualTo(mapWidth));
                Assert.That(result.MapHeight, Is.EqualTo(mapHeight));
            }

            [Test]
            public void It_should_set_player_position()
            {
                Assert.That(result.X, Is.EqualTo(23));
                Assert.That(result.Y, Is.EqualTo(24));
            }

            [Test]
            public void It_should_return_list_of_players() => Assert.That(result.Players, Is.Not.Null);

            [Test]
            public void It_should_use_map_dimensions_to_generate_start_position()
            {
                GetMock<IRandom>().Verify(x => x.Next(mapWidth));
                GetMock<IRandom>().Verify(x => x.Next(mapHeight));
            }

            [Test]
            public void It_should_save_the_new_player() => GetMock<IPlayerRepository>().Verify(x => x.Save(IsAny<Player>()));
        }

        public class When_registering_multiple_players :WithAnAutomocked<RegisterPlayerCommand>
        {
            private CommandResult firstResult;
            private CommandResult secondResult;

            [OneTimeSetUp]
            public void SetUp()
            {
                var playerCount = 0;
                GetMock<IPlayerRepository>().Setup(x => x.GetPlayerCount()).Returns(() => playerCount++);

                firstResult = ClassUnderTest.Execute();
                secondResult = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_set_first_player_as_it() => Assert.That(firstResult.IsIt, Is.True);

            [Test]
            public void It_should_set_second_player_as_NOT_it() => Assert.That(secondResult.IsIt, Is.False);
        }
    }
}