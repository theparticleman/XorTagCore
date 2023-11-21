namespace XorTag.Domain;

public interface IIdGenerator
{
    int GenerateId(IEnumerable<int> existingIds);
}

public class IdGenerator : IIdGenerator
{
    public const int IdBase = 1000;
    private readonly IRandom random;

    public IdGenerator(IRandom random)
    {
        this.random = random;
    }

    public int GenerateId(IEnumerable<int> existingIds)
    {
        int generatedId;
        var existingIdsAsArray = existingIds.ToHashSet();
        do
        {
            generatedId = IdBase + random.Next(1000);
        } while (existingIdsAsArray.Contains(generatedId));
        return generatedId;
    }
}