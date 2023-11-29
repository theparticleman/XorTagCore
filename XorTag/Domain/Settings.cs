namespace XorTag.Domain;

public interface ISettings
{
    int MapWidth { get; }
    int MapHeight { get; }
    int InactivityTimeoutMilliseconds { get; }
}

public class Settings : ISettings
{
    public int MapWidth => 50;
    public int MapHeight => 30;
    public int InactivityTimeoutMilliseconds => 5 * 60 * 1000;
}