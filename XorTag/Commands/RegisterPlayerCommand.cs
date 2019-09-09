using System;
using System.Collections.Generic;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class RegisterPlayerCommand
    {
        private readonly IIdGenerator idGenerator;
        private readonly INameGenerator nameGenerator;
        private readonly IMapSettings mapSettings;
        private readonly IRandom random;
        private readonly IPlayerRepository playerRepository;

        public RegisterPlayerCommand(IIdGenerator idGenerator, INameGenerator nameGenerator, IMapSettings mapSettings, IRandom random, IPlayerRepository playerRepository)
        {
            this.idGenerator = idGenerator;
            this.nameGenerator = nameGenerator;
            this.mapSettings = mapSettings;
            this.random = random;
            this.playerRepository = playerRepository;
        }
        public CommandResult Execute()
        {
            var playerCount = playerRepository.GetPlayerCount();
            var player = new Player
            {
                Id = idGenerator.GenerateId(new int[] { }),
                Name = nameGenerator.GenerateName(new string[] { }),
                X = random.Next(mapSettings.MapWidth),
                Y = random.Next(mapSettings.MapHeight),
                IsIt = playerCount == 0
            };
            playerRepository.Save(player);
            return new CommandResult
            {
                Name = player.Name,
                Id = player.Id,
                IsIt = player.IsIt,
                MapWidth = mapSettings.MapWidth,
                MapHeight = mapSettings.MapHeight,
                X = player.X,
                Y = player.Y,
                Players = new List<PlayerResult>()
            };
        }
    }
}