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
            AdjustPlayerPosition(currentPlayer, direction);
            playerRepository.UpdatePlayerPosition(currentPlayer);
            return new CommandResult
            {
                X = currentPlayer.X,
                Y = currentPlayer.Y
            };
        }

        private void AdjustPlayerPosition(Player currentPlayer, string direction)
        {
            switch (direction)
            {
                case "up":
                    currentPlayer.Y -= 1;
                    break;
                case "down":
                    currentPlayer.Y += 1;
                    break;
                case "left":
                    currentPlayer.X -= 1;
                    break;
                case "right":
                    currentPlayer.X += 1;
                    break;
            }
        }
    }
}