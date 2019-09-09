using System;
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
            if (currentPlayer.Y < 0) currentPlayer.Y = 0;
            if (currentPlayer.Y >= mapSettings.MapHeight) currentPlayer.Y = mapSettings.MapHeight - 1;
            if (currentPlayer.X < 0) currentPlayer.X = 0;
            if (currentPlayer.X >= mapSettings.MapWidth) currentPlayer.X = mapSettings.MapWidth - 1;
        }
    }
}