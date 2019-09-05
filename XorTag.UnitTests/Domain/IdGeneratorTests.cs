using Moq;
using NUnit.Framework;
using XorTag.Domain;

namespace XorTag.UnitTests.Domain
{
    public class IdGeneratorTests
    {
        public class When_generating_id_with_no_existing_ids
        {
            [Test]
            public void It_should_generate_an_id()
            {
                var random = new Mock<IRandom>();
                var classUnderTest = new IdGenerator(random.Object);
                var generatedId = classUnderTest.GenerateId(new int[] { });
                Assert.That(generatedId, Is.GreaterThanOrEqualTo(IdGenerator.IdBase));
            }
        }

        public class When_generating_id_with_existing_ids
        {
            [Test]
            public void It_should_generate_a_unique_id()
            {
                var random = new Mock<IRandom>();
                int randomValue = 0;
                random.Setup(x => x.Next(It.IsAny<int>())).Returns(() => ++randomValue);
                var classUnderTest = new IdGenerator(random.Object);
                var expectedId = IdGenerator.IdBase + 2;
                var generatedId = classUnderTest.GenerateId(new int[] { IdGenerator.IdBase + 1 });
                Assert.That(generatedId, Is.EqualTo(expectedId));
            }
        }
    }
}