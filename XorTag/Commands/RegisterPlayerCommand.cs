using System;
using System.Collections.Generic;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class RegisterPlayerCommand
    {
        private readonly IIdGenerator idGenerator;
        private readonly INameGenerator nameGenerator;
        private readonly IPlayerStartLocation playerStartLocation;

        public RegisterPlayerCommand(IIdGenerator idGenerator, INameGenerator nameGenerator, IPlayerStartLocation playerStartLocation)
        {
            this.idGenerator = idGenerator;
            this.nameGenerator = nameGenerator;
            this.playerStartLocation = playerStartLocation;
        }
        public CommandResult Execute()
        {
            var startLocation = playerStartLocation.Generate();
            return new CommandResult
            {
                Name = nameGenerator.GenerateName(new string[] { }),
                Id = idGenerator.GenerateId(new int[] { }),
                IsIt = true,
                MapWidth = 50,
                MapHeight = 30,
                X = startLocation.x,
                Y = startLocation.y,
                Players = new List<PlayerResult>()
            };
        }
    }
}