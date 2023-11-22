using XorTag.Domain;

namespace XorTag.Commands;

public class LookCommand(IPlayerRepository playerRepository, IMapSettings mapSettings)
{
  private readonly IPlayerRepository playerRepository = playerRepository;
  private readonly IMapSettings mapSettings = mapSettings;

  public CommandResult Execute(int playerId)
  {
    var allPlayers = playerRepository.GetAllPlayers();
    var currentPlayer = allPlayers.Single(x => x.Id == playerId);
    return new CommandResult
    {
      Id = currentPlayer.Id,
      Name = currentPlayer.Name,
      X = currentPlayer.X,
      Y = currentPlayer.Y,
      IsIt = currentPlayer.IsIt,
      MapWidth = mapSettings.MapWidth,
      MapHeight = mapSettings.MapHeight,
      Players = [],
    };
  }
}