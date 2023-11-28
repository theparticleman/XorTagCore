namespace XorTag.Domain;

public interface IPlayerRepository
{
    List<Player> GetAllPlayers();
    void UpdatePlayerPosition(Player player);
    void SavePlayerAsIt(int playerId);
    void SavePlayerAsNotIt(int playerId);
    void Save(Player player);
    int GetPlayerCount();
    void ClearAllPlayers();
}

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> players = [];
    private readonly Dictionary<int, DateTimeOffset> lastActiveLookup = [];

    public void ClearAllPlayers()
    {
        players.Clear();
    }

    public List<Player> GetAllPlayers()
    {
        return players.ToList();
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    public void Save(Player player)
    {
        var playerCopy = new Player
        {
            Id = player.Id,
            Name = player.Name,
            X = player.X,
            Y = player.Y,
            IsIt = player.IsIt
        };
        players.Add(playerCopy);
    }

    public void SavePlayerAsIt(int playerId)
    {
        var playerToUpdate = players.FirstOrDefault(x => x.Id == playerId);
        if (playerToUpdate != null) playerToUpdate.IsIt = true;
    }

    public void SavePlayerAsNotIt(int playerId)
    {
        var playerToUpdate = players.FirstOrDefault(x => x.Id == playerId);
        if (playerToUpdate != null) playerToUpdate.IsIt = false;
    }

    public void UpdatePlayerPosition(Player player)
    {
        var playerToUpdate = players.FirstOrDefault(x => x.Id == player.Id);
        if (playerToUpdate == null) return;
        playerToUpdate.X = player.X;
        playerToUpdate.Y = player.Y;
    }

    public DateTimeOffset GetLastActiveTime(int playerId)
    {
        if (lastActiveLookup.TryGetValue(playerId, out var lastActiveTime)) return lastActiveTime;
        return DateTimeOffset.MinValue;
    }

    public void UpdateLastActiveTime(int playerId)
    {
        lastActiveLookup[playerId] = DateTimeOffset.Now;
    }

    public void RemovePlayer(int playerId)
    {
        var index = players.FindIndex(x => x.Id == playerId);
        if (index <= -1) return;
        players.RemoveAt(index);
    }
}