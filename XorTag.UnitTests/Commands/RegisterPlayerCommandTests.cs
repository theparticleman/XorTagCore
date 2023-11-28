using XorTag.Commands;

namespace XorTag.UnitTests.Commands;

public class RegisterPlayerCommandTests
{
    public class When_registering_a_new_player : WithAnAutomocked<RegisterPlayerCommand>
    {
        private const string name = "generated-name";
        private const int mapWidth = 40;
        private const int mapHeight = 20;
        private static readonly List<Player> existingPlayers = new List<Player>
            {
                new Player { Id = 1, Name = "Name 1" },
                new Player { Id = 2, Name = "Name 2" },
            };

        private CommandResult result;
        private IEnumerable<int> capturedIdList = null;
        private IEnumerable<string> capturedNameList = null;

        [OneTimeSetUp]
        public void SetUp()
        {
            GetMock<IIdGenerator>().Setup(x => x.GenerateId(IsAny<IEnumerable<int>>()))
                .Callback<IEnumerable<int>>(x => capturedIdList = x)
                .Returns(1234);
            GetMock<INameGenerator>().Setup(x => x.GenerateName(IsAny<IEnumerable<string>>()))
                .Callback<IEnumerable<string>>(x => capturedNameList = x)
                .Returns(name);
            var randomValue = 23;
            GetMock<IRandom>().Setup(x => x.Next(IsAny<int>())).Returns(() => randomValue++);
            GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(existingPlayers);
            result = ClassUnderTest.Execute();
        }

        [Test]
        public void It_should_generate_a_new_id() => Assert.That(result.Id, Is.EqualTo(1234));

        [Test]
        public void It_should_generate_a_name() => Assert.That(result.Name, Is.EqualTo(name));

        [Test]
        public void It_should_make_the_player_NOT_it() => Assert.That(result.IsIt, Is.False);

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

        [Test]
        public void It_should_use_the_existing_player_ids_when_generating_new_player_id()
        {
            var existingPlayerIds = existingPlayers.Select(x => x.Id);
            Assert.That(capturedIdList, Is.EquivalentTo(existingPlayerIds));
        }

        [Test]
        public void It_should_use_the_existing_player_names_when_generating_new_player_name()
        {
            var existingPlayerNames = existingPlayers.Select(x => x.Name);
            Assert.That(capturedNameList, Is.EquivalentTo(existingPlayerNames));
        }
    }

    public class When_registering_multiple_players
    {
        private CommandResult firstResult;
        private CommandResult secondResult;

        [OneTimeSetUp]
        public void SetUp()
        {
            var idGeneratorMock = new Mock<IIdGenerator>();
            var nameGeneratorMock = new Mock<INameGenerator>();
            var mapSettingsMock = new Mock<IMapSettings>();
            var randomMock = new Mock<IRandom>();
            var playerRepository = new InMemoryPlayerRepository();

            var classUnderTest = new RegisterPlayerCommand(idGeneratorMock.Object, nameGeneratorMock.Object, mapSettingsMock.Object, randomMock.Object, playerRepository);

            firstResult = classUnderTest.Execute();
            secondResult = classUnderTest.Execute();
        }

        [Test]
        public void It_should_set_first_player_as_it() => Assert.That(firstResult.IsIt, Is.True);

        [Test]
        public void It_should_set_second_player_as_NOT_it() => Assert.That(secondResult.IsIt, Is.False);
    }
}