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

        public RegisterPlayerCommand(IIdGenerator idGenerator, INameGenerator nameGenerator, IMapSettings mapSettings, IRandom random)
        {
            this.idGenerator = idGenerator;
            this.nameGenerator = nameGenerator;
            this.mapSettings = mapSettings;
            this.random = random;
        }
        public CommandResult Execute()
        {
            return new CommandResult
            {
                Name = nameGenerator.GenerateName(new string[] { }),
                Id = idGenerator.GenerateId(new int[] { }),
                IsIt = true,
                MapWidth = mapSettings.MapWidth,
                MapHeight = mapSettings.MapHeight,
                X = random.Next(mapSettings.MapWidth),
                Y = random.Next(mapSettings.MapHeight),
                Players = new List<PlayerResult>()
            };
        }
    }
}