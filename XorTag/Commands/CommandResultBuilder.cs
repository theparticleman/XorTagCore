using XorTag.Domain;

namespace XorTag.Commands;

public interface ICommandResultBuilder
{
  CommandResult Build(Player player, List<Player> allPlayers);
}

public class CommandResultBuilder(IMapSettings mapSettings) : ICommandResultBuilder
{
  public const int LookRadius = 5;
  private readonly IMapSettings mapSettings = mapSettings;

  public CommandResult Build(Player player, List<Player> allPlayers)
  {
    return new CommandResult
    {
      X = player.X,
      Y = player.Y,
      Id = player.Id,
      Name = player.Name,
      IsIt = player.IsIt,
      MapWidth = mapSettings.MapWidth,
      MapHeight = mapSettings.MapHeight,
      Players = allPlayers
        .Where(p => p.Id != player.Id)
        .Where(p => Math.Abs(p.X - player.X) < LookRadius)
        .Where(p => Math.Abs(p.Y - player.Y) < LookRadius)
        .Select(p => new PlayerResult
        {
          X = p.X,
          Y = p.Y,
          IsIt = p.IsIt,
        })
        .ToList(),
    };
  }
}