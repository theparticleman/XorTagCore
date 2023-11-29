
namespace XorTag.Domain;

public class ActionFrequencyChecker(ISettings settings)
{
  private readonly Dictionary<int, DateTimeOffset> lastActionTimeLookup = [];
  private readonly ISettings settings = settings;

  public void CheckFreqency(int playerId)
  {
    if (lastActionTimeLookup.TryGetValue(playerId, out var lastActionTime))
    {
      Console.WriteLine("lastActionTime: " + lastActionTime);
      TimeSpan difference = DateTimeOffset.Now - lastActionTime;
      Console.WriteLine("difference: " + difference.TotalMilliseconds + "ms");
      if (difference.TotalMilliseconds <= settings.MaxActionFrequencyMilliseconds)
      {
        throw new TooFrequentActionException();
      }
    }
    lastActionTimeLookup[playerId] = DateTimeOffset.Now;
  }
}