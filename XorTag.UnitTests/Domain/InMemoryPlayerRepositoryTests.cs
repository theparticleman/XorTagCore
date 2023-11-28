namespace XorTag.UnitTests.Domain;

public class InMemoryPlayerRepositoryTests
{
    public class When_saving_new_player
    {
        private InMemoryPlayerRepository classUnderTest;
        private IEnumerable<Player> allPlayers;
        private Player retrievedPlayer;
        private Player player = new Player { Id = 1234, Name = "player-name", X = 32, Y = 13, IsIt = true };

        [OneTimeSetUp]
        public void SetUp()
        {
            classUnderTest = new InMemoryPlayerRepository();
            classUnderTest.Save(player);
            allPlayers = classUnderTest.GetAllPlayers();
            retrievedPlayer = allPlayers.FirstOrDefault(x => x.Id == player.Id);
        }

        [Test]
        public void It_should_be_able_to_retrieve_saved_player() => Assert.That(retrievedPlayer, Is.Not.Null);

        [Test]
        public void It_should_save_player_name() => Assert.That(retrievedPlayer.Name, Is.EqualTo(player.Name));

        [Test]
        public void It_should_save_player_position()
        {
            Assert.That(retrievedPlayer.X, Is.EqualTo(player.X));
            Assert.That(retrievedPlayer.Y, Is.EqualTo(player.Y));
        }

        [Test]
        public void It_should_save_is_it_status() => Assert.That(retrievedPlayer.IsIt, Is.EqualTo(player.IsIt));
    }

    public class When_updating_player_position
    {
        private Player player = new Player { Id = 1234, Name = "player-name", X = 32, Y = 13, IsIt = true };

        [Test]
        public void It_should_update_the_player_position()
        {
            var classUnderTest = new InMemoryPlayerRepository();
            classUnderTest.Save(player);
            player.X = 42;
            player.Y = 42;

            classUnderTest.UpdatePlayerPosition(player);

            var allPlayers = classUnderTest.GetAllPlayers();
            var retrievedPlayer = allPlayers.FirstOrDefault(x => x.Id == player.Id);
            Assert.That(retrievedPlayer.X, Is.EqualTo(42));
            Assert.That(retrievedPlayer.Y, Is.EqualTo(42));
        }
    }

    public class When_saving_player_as_it
    {
        private Player player = new Player { Id = 1234, Name = "player-name", X = 32, Y = 13, IsIt = false };

        [Test]
        public void It_should_persist_is_it_state()
        {
            var classUnderTest = new InMemoryPlayerRepository();
            classUnderTest.Save(player);

            classUnderTest.SavePlayerAsIt(player.Id);

            var allPlayers = classUnderTest.GetAllPlayers();
            var updatedPlayer = allPlayers.Single(x => x.Id == player.Id);

            Assert.That(updatedPlayer.IsIt, Is.True);
        }
    }

    public class When_saving_player_as_NOT_it
    {
        private Player player = new Player { Id = 1234, Name = "player-name", X = 32, Y = 13, IsIt = true };

        [Test]
        public void It_should_persist_is_it_state()
        {
            var classUnderTest = new InMemoryPlayerRepository();
            classUnderTest.Save(player);

            classUnderTest.SavePlayerAsNotIt(player.Id);

            var allPlayers = classUnderTest.GetAllPlayers();
            var updatedPlayer = allPlayers.Single(x => x.Id == player.Id);

            Assert.That(updatedPlayer.IsIt, Is.False);
        }
    }
}