using System.Threading;

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

    public class When_updating_last_active_time
    {
        [Test]
        public void It_should_update()
        {
            var classUnderTest = new InMemoryPlayerRepository();
            var player = new Player
            {
                Id = 1234,
            };
            classUnderTest.Save(player);
            var initialLastActiveTime = classUnderTest.GetLastActiveTime(player.Id);

            classUnderTest.UpdateLastActiveTime(player.Id);

            var updatedLastActiveTime = classUnderTest.GetLastActiveTime(player.Id);

            Assert.That(updatedLastActiveTime, Is.Not.EqualTo(initialLastActiveTime));
        }

        [Test]
        public void It_should_only_update_when_called()
        {
            var classUnderTest = new InMemoryPlayerRepository();
            var player = new Player
            {
                Id = 1234,
            };
            classUnderTest.Save(player);
            classUnderTest.UpdateLastActiveTime(player.Id);

            var firstLastActiveTime = classUnderTest.GetLastActiveTime(player.Id);

            Thread.Sleep(50);

            var secondLastActiveTime = classUnderTest.GetLastActiveTime(player.Id);

            Assert.That(firstLastActiveTime, Is.EqualTo(secondLastActiveTime));
        }
    }

    public class When_removing_player : WithAnAutomocked<InMemoryPlayerRepository>
    {
        [Test]
        public void It_should_remove_the_player()
        {
            var player = new Player
            {
                Id = 1234,
            };

            ClassUnderTest.Save(player);
            var allPlayers = ClassUnderTest.GetAllPlayers();
            Assert.That(allPlayers, Has.Count.EqualTo(1));

            ClassUnderTest.RemovePlayer(player.Id);

            allPlayers = ClassUnderTest.GetAllPlayers();
            Assert.That(allPlayers, Has.Count.EqualTo(0));
        }
    }
}