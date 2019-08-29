using XorTag.Commands;
using NUnit.Framework;
using XorTag.Domain;

namespace XorTag.UnitTests
{
    public class RegisterPlayerCommandTests
    {
        public class When_registering_a_new_player: WithAnAutomocked<RegisterPlayerCommand>
        {
            private CommandResult result;

            [SetUp]
            public void SetUp()
            {
                GetMock<IIdGenerator>().Setup(x => x.GenerateId()).Returns(1234);
                result = ClassUnderTest.Execute();
            }

            [Test]
            public void It_should_generate_a_new_id() => Assert.That(result.Id, Is.EqualTo(1234));
        }
    }
}