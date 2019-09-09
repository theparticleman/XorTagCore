using System.Collections.Generic;

namespace XorTag.Domain
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetAllPlayers();
        void UpdatePlayerPosition(Player player);
        void SavePlayerAsIt(int playerId);
        void SavePlayerAsNotIt(int playerId);
    }
}