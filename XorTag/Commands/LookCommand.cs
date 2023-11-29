using XorTag.Domain;

namespace XorTag.Commands;

public class LookCommand(IPlayerRepository playerRepository, ICommandResultBuilder commandResultBuilder)
{
  private readonly IPlayerRepository playerRepository = playerRepository;
  private readonly ICommandResultBuilder commandResultBuilder = commandResultBuilder;

  public CommandResult Execute(int playerId)
  {
    var allPlayers = playerRepository.GetAllPlayers();
    var currentPlayer = allPlayers.SingleOrDefault(x => x.Id == playerId);
    if (currentPlayer == null) throw new NotFoundException();
    playerRepository.UpdateLastActiveTime(currentPlayer.Id);
    return commandResultBuilder.Build(currentPlayer, allPlayers);
  }
}