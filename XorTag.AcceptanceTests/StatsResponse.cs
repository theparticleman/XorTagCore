namespace XorTag.AcceptanceTests
{
    internal class StatsResponse
    {
        public string WinningPlayerName { get; set; }
        public string IsItPlayerName { get; set; }
        public PlayerStats[] Players { get; set; }
    }

    public class PlayerStats
    {
        public string Name { get; set; }
    }
}