using System;
using System.Collections.Generic;
using System.Linq;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class MovePlayerCommand
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IMapSettings mapSettings;

        public MovePlayerCommand(IPlayerRepository playerRepository, IMapSettings mapSettings)
        {
            this.playerRepository = playerRepository;
            this.mapSettings = mapSettings;
        }

        public CommandResult Execute(string direction, int playerId)
        {
            var allPlayers = playerRepository.GetAllPlayers();
            var currentPlayer = allPlayers.Single(x => x.Id == playerId);
            AdjustPlayerPosition(currentPlayer, direction, allPlayers);
            playerRepository.UpdatePlayerPosition(currentPlayer);
            return new CommandResult
            {
                X = currentPlayer.X,
                Y = currentPlayer.Y
            };
        }

        private void AdjustPlayerPosition(Player currentPlayer, string direction, IEnumerable<Player> allPlayers)
        {
            int newX = currentPlayer.X;
            int newY = currentPlayer.Y;
            switch (direction)
            {
                case "up":
                    newY -= 1;
                    break;
                case "down":
                    newY += 1;
                    break;
                case "left":
                    newX -= 1;
                    break;
                case "right":
                    newX += 1;
                    break;
            }
            if (IsNewPositionValid(newX, newY, allPlayers))
            {
                currentPlayer.X = newX;
                currentPlayer.Y = newY;
            }
        }

        private bool IsNewPositionValid(int newX, int newY, IEnumerable<Player> allPlayers)
        {
            if (
                newY < 0 || newY >= mapSettings.MapHeight ||
                newX < 0 || newX >= mapSettings.MapWidth)
            {
                return false;
            }
            return allPlayers.Count(p => p.X == newX && p.Y == newY) == 0;
        }
    }
}