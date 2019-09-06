using System.Collections.Generic;

namespace XorTag.AcceptanceTests
{
    public class ApiResult
    {
        public bool IsIt { get; set; }
        public int MapHeight { get; set; }
        public int MapWidth { get; set; }
        public string Name { get; set; }
        public List<PlayerResult> Players { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; }
    }

    public class PlayerResult
    {
        public bool IsIt { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}