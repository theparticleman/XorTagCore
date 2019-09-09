using System.Collections.Generic;
using System.Linq;

namespace XorTag.Domain
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetAllPlayers();
        void UpdatePlayerPosition(Player player);
        void SavePlayerAsIt(int playerId);
        void SavePlayerAsNotIt(int playerId);
        void Save(Player player);
    }

    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private readonly List<Player> players = new List<Player>();

        public IEnumerable<Player> GetAllPlayers()
        {
            return players.ToArray();
        }

        public void Save(Player player)
        {
            var playerCopy = new Player
            {
                Id = player.Id,
                Name = player.Name,
                X = player.X,
                Y = player.Y,
                IsIt = player.IsIt
            };
            players.Add(playerCopy);
        }

        public void SavePlayerAsIt(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public void SavePlayerAsNotIt(int playerId)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePlayerPosition(Player player)
        {
            var playerToUpdate = players.FirstOrDefault(x => x.Id == player.Id);
            if (playerToUpdate == null) return;
            playerToUpdate.X = player.X;
            playerToUpdate.Y = player.Y;
        }
    }
}