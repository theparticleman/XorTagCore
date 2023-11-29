namespace XorTag.Domain;

public class PlayerInactivityChecker(IPlayerRepository playerRepository, ISettings settings, ILogger<PlayerInactivityChecker> logger) : IHostedService
{
  private readonly IPlayerRepository playerRepository = playerRepository;
  private readonly ISettings settings = settings;
  private readonly ILogger logger = logger;
  private bool running = false;
  private Task task;

  public Task StartAsync(CancellationToken cancellationToken)
  {
    logger.LogInformation("start player inactivity checker");
    running = true;
    task = new Task(Run);
    task.Start();
    return Task.CompletedTask;
  }

  private void Run()
  {
    while (running)
    {
      var allPlayers = playerRepository.GetAllPlayers();
      foreach (var player in allPlayers)
      {
        var lastActiveTime = playerRepository.GetLastActiveTime(player.Id);
        if (lastActiveTime == DateTimeOffset.MinValue) continue;
        var elapsedTime = DateTimeOffset.Now - lastActiveTime;
        if (elapsedTime.TotalMilliseconds > settings.InactivityTimeoutMilliseconds)
        {
          logger.LogInformation($"Removing player id {player.Id}");
          playerRepository.RemovePlayer(player.Id);
        }
      }
      Thread.Sleep(10);
    }
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    logger.LogInformation("stop player inactivity checker");
    running = false;
    return Task.CompletedTask;
  }
}