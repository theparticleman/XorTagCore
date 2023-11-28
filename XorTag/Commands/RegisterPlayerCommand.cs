using XorTag.Domain;

namespace XorTag.Commands;

public class RegisterPlayerCommand(
    IIdGenerator idGenerator,
    INameGenerator nameGenerator,
    IMapSettings mapSettings,
    IRandom random,
    IPlayerRepository playerRepository,
    ICommandResultBuilder commandResultBuilder)
{
    private readonly IIdGenerator idGenerator = idGenerator;
    private readonly INameGenerator nameGenerator = nameGenerator;
    private readonly IMapSettings mapSettings = mapSettings;
    private readonly IRandom random = random;
    private readonly IPlayerRepository playerRepository = playerRepository;
    private readonly ICommandResultBuilder commandResultBuilder = commandResultBuilder;

    public CommandResult Execute()
    {
        var existingPlayers = playerRepository.GetAllPlayers();
        var player = new Player
        {
            Id = idGenerator.GenerateId(existingPlayers.Select(x => x.Id)),
            Name = nameGenerator.GenerateName(existingPlayers.Select(x => x.Name)),
            X = random.Next(mapSettings.MapWidth),
            Y = random.Next(mapSettings.MapHeight),
            IsIt = existingPlayers.Count() == 0,
        };
        playerRepository.Save(player);
        return commandResultBuilder.Build(player, existingPlayers);
    }
}