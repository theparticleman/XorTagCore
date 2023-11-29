using XorTag.Commands;
using XorTag.Domain;

namespace XorTag.UnitTests.Commands;

public class RegisterPlayerCommandTests
{
    public class When_registering_a_new_player : WithAnAutomocked<RegisterPlayerCommand>
    {
        private const string generatedName = "generated-name";
        private const int mapWidth = 40;
        private const int mapHeight = 20;
        const int generatedId = 1234;
        private static readonly List<Player> existingPlayers = new List<Player>
            {
                new Player { Id = 1, Name = "Name 1" },
                new Player { Id = 2, Name = "Name 2" },
            };

        private CommandResult result;
        private IEnumerable<int> capturedIdList = null;
        private IEnumerable<string> capturedNameList = null;
        CommandResult builtCommandResult = new();
        Player capturedPlayer;

        [OneTimeSetUp]
        public void SetUp()
        {
            GetMock<IIdGenerator>().Setup(x => x.GenerateId(IsAny<IEnumerable<int>>()))
                .Callback<IEnumerable<int>>(x => capturedIdList = x)
                .Returns(generatedId);
            GetMock<INameGenerator>().Setup(x => x.GenerateName(IsAny<IEnumerable<string>>()))
                .Callback<IEnumerable<string>>(x => capturedNameList = x)
                .Returns(generatedName);
            var randomValue = 23;
            GetMock<IRandom>().Setup(x => x.Next(IsAny<int>())).Returns(() => randomValue++);
            GetMock<ISettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<ISettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(existingPlayers);
            GetMock<ICommandResultBuilder>()
                .Setup(x => x.Build(IsAny<Player>(), existingPlayers))
                .Callback<Player, List<Player>>((p, _) => capturedPlayer = p)
                .Returns(builtCommandResult);
            result = ClassUnderTest.Execute();
        }

        [Test]
        public void It_should_generate_a_new_id() => Assert.That(capturedPlayer.Id, Is.EqualTo(generatedId));

        [Test]
        public void It_should_generate_a_name() => Assert.That(capturedPlayer.Name, Is.EqualTo(generatedName));

        [Test]
        public void It_should_make_the_player_NOT_it() => Assert.That(capturedPlayer.IsIt, Is.False);

        [Test]
        public void It_should_return_the_result_from_the_builder() => Assert.That(result, Is.EqualTo(builtCommandResult));

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

        [Test]
        public void It_should_update_last_active_time() => GetMock<IPlayerRepository>().Verify(x => x.UpdateLastActiveTime(generatedId));

        [Test]
        public void It_should_check_last_action_time() => GetMock<IActionFrequencyChecker>().Verify(x => x.CheckFreqency(generatedId));
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
            var settingsMock = new Mock<ISettings>();
            var randomMock = new Mock<IRandom>();
            var playerRepository = new InMemoryPlayerRepository();
            var commandResultBuilder = new CommandResultBuilder(settingsMock.Object);
            var actionFrequencyCheckerMock = new Mock<IActionFrequencyChecker>();

            var classUnderTest = new RegisterPlayerCommand(
                idGeneratorMock.Object, nameGeneratorMock.Object, settingsMock.Object, 
                randomMock.Object, playerRepository, commandResultBuilder, actionFrequencyCheckerMock.Object);

            firstResult = classUnderTest.Execute();
            secondResult = classUnderTest.Execute();
        }

        [Test]
        public void It_should_set_first_player_as_it() => Assert.That(firstResult.IsIt, Is.True);

        [Test]
        public void It_should_set_second_player_as_NOT_it() => Assert.That(secondResult.IsIt, Is.False);
    }
}