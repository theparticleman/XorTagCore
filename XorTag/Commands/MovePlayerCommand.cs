using System;
using System.Linq;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class MovePlayerCommand
    {
        private readonly IPlayerRepository playerRepository;

        public MovePlayerCommand(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public CommandResult Execute(string direction, int playerId)
        {
            var allPlayers = playerRepository.GetAllPlayers();
            var currentPlayer = allPlayers.Single(x => x.Id == playerId);
            return new CommandResult
            {
                Y = currentPlayer.Y - 1
            };
        }
    }
}