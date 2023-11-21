namespace XorTag.Domain;

public interface IRandom
{
    int Next(int max);
}

public class Random : IRandom
{
    private static readonly System.Random rand = new System.Random();

    public int Next(int max)
    {
        return rand.Next(max);
    }
}