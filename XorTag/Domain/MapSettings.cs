namespace XorTag.Domain;

public interface IMapSettings
{
    int MapWidth { get; }
    int MapHeight { get; }
}

public class MapSettings : IMapSettings
{
    public int MapWidth => 50;
    public int MapHeight => 30;
}