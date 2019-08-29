using System;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class RegisterPlayerCommand
    {
        private readonly IIdGenerator idGenerator;

        public RegisterPlayerCommand(IIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator;
        }
        public CommandResult Execute()
        {
            return new CommandResult
            {
                Id = idGenerator.GenerateId()
            };
        }
    }
}