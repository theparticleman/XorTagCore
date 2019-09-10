using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers
{
    [ApiController]
    public class StatsController: ControllerBase
    {
        private readonly StatsCommand statsCommand;

        public StatsController(StatsCommand statsCommand)
        {
            this.statsCommand = statsCommand;
        }

        [Route("/stats")]
        public StatsResult Get()
        {
            return statsCommand.Execute();
        }
    }
}