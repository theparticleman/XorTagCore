using XorTag.Commands;
using NUnit.Framework;
using XorTag.Domain;
using System.Collections.Generic;

namespace XorTag.UnitTests
{
    public class RegisterPlayerCommandTests
    {
        public class When_registering_a_new_player: WithAnAutomocked<RegisterPlayerCommand>
        {
            private CommandResult result;
            private const string name = "generated-name";

            [SetUp]
            public void SetUp()
            {
                GetMock<IIdGenerator>().Setup(x => x.GenerateId(IsAny<IEnumerable<int>>())).Returns(1234);
                GetMock<INameGenerator>().Setup(x => x.GenerateName(IsAny<IEnumerable<string>>())).Returns(name);
                GetMock<IPlayerStartLocation>().Setup(x => x.Generate()).Returns((23, 31));
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
                Assert.That(result.MapWidth, Is.EqualTo(50));
                Assert.That(result.MapHeight, Is.EqualTo(30));
            }

            [Test]
            public void It_should_set_player_position()
            {
                Assert.That(result.X, Is.EqualTo(23));
                Assert.That(result.Y, Is.EqualTo(31));
            }

            [Test]
            public void It_should_return_list_of_players() => Assert.That(result.Players, Is.Not.Null);
        }
    }
}