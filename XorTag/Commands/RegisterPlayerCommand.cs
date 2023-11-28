using XorTag.Domain;

namespace XorTag.Commands;

public class RegisterPlayerCommand(
    IIdGenerator idGenerator,
    INameGenerator nameGenerator,
    ISettings settings,
    IRandom random,
    IPlayerRepository playerRepository,
    ICommandResultBuilder commandResultBuilder)
{
    private readonly IIdGenerator idGenerator = idGenerator;
    private readonly INameGenerator nameGenerator = nameGenerator;
    private readonly ISettings settings = settings;
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
            X = random.Next(settings.MapWidth),
            Y = random.Next(settings.MapHeight),
            IsIt = existingPlayers.Count() == 0,
        };
        playerRepository.Save(player);
        return commandResultBuilder.Build(player, existingPlayers);
    }
}