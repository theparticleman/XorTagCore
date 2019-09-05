namespace XorTag.Domain
{
    public interface IPlayerStartLocation
    {
        (int x, int y) Generate();
    }
}