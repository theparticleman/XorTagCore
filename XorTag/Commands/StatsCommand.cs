using System;
using System.Collections.Generic;

namespace XorTag.Commands
{
    public class StatsCommand
    {
        public StatsResult Execute()
        {
            return new StatsResult();
        }
    }

    public class StatsResult
    {
        public StatsResult()
        {
            Players = new List<PlayerStats>();
        }
        public string WinningPlayerName { get; set; }
        public string IsItPlayerName { get; set; }
        public List<PlayerStats> Players { get; set; }
    }

    public class PlayerStats
    {
    }
}