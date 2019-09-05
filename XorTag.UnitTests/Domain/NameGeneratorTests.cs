using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using XorTag.Domain;

namespace XorTag.UnitTests.Domain
{
    public class NameGeneratorTests
    {
        public class When_generating_name_with_no_existing_names
        {
            private NameGenerator classUnderTest;
            private string generatedName;

            [OneTimeSetUp]
            public void SetUp()
            {
                classUnderTest = new NameGenerator();
                generatedName = classUnderTest.GenerateName(new string[] { });
            }

            [Test]
            public void It_should_generate_a_name_from_list() => Assert.That(NameGenerator.AllNames, Does.Contain(generatedName));
        }

        public class When_generating_name_with_existing_names
        {
            private NameGenerator classUnderTest;
            private IEnumerable<string> existingNames;
            private string expectedName;
            private string generatedName;

            [OneTimeSetUp]
            public void SetUp()
            {
                classUnderTest = new NameGenerator();
                existingNames = NameGenerator.AllNames.Skip(1);
                expectedName = NameGenerator.AllNames.First();
                generatedName = classUnderTest.GenerateName(existingNames);
            }

            [Test]
            public void It_should_generate_the_only_unused_name() => Assert.That(generatedName, Is.EqualTo(expectedName));
        }
    }
}