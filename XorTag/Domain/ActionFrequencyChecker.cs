
namespace XorTag.Domain;

public interface IActionFrequencyChecker
{
  void CheckFreqency(int playerId);
}

public class ActionFrequencyChecker(ISettings settings) : IActionFrequencyChecker
{
  private readonly Dictionary<int, DateTimeOffset> lastActionTimeLookup = [];
  private readonly ISettings settings = settings;

  public void CheckFreqency(int playerId)
  {
    if (lastActionTimeLookup.TryGetValue(playerId, out var lastActionTime))
    {
      if ((DateTimeOffset.Now - lastActionTime).TotalMilliseconds <= settings.MaxActionFrequencyMilliseconds)
      {
        throw new TooFrequentActionException();
      }
    }
    lastActionTimeLookup[playerId] = DateTimeOffset.Now;
  }
}