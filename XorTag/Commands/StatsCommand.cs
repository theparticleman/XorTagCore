using System;
using System.Collections.Generic;
using System.Linq;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class StatsCommand
    {
        private readonly IPlayerRepository playerRepository;

        public StatsCommand(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public StatsResult Execute()
        {
            var allPlayers = playerRepository.GetAllPlayers();
            var isItPlayer = allPlayers.FirstOrDefault(x => x.IsIt);
            var winningPlayer = allPlayers.FirstOrDefault(x => !x.IsIt);
            return new StatsResult
            {
                IsItPlayerName = isItPlayer?.Name,
                WinningPlayerName = winningPlayer?.Name,
                Players = allPlayers.Where(x => !x.IsIt).Select(x => new PlayerStats { Name = x.Name }).ToList()
            };
        }
    }

    public class StatsResult
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