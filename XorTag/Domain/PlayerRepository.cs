using System.Collections.Generic;

namespace XorTag.Domain
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetAllPlayers();
        void UpdatePlayerPosition(Player player);
    }
}