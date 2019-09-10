using System.Collections.Generic;

namespace XorTag.AcceptanceTests
{
    internal class StatsResponse
    {
        public string WinningPlayerName { get; set; }
        public string IsItPlayerName { get; set; }
        public List<PlayerStats> Players { get; set; }
    }

    public class PlayerStats
    {
        public string Name { get; set; }
    }
}