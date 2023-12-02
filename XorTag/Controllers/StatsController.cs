using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using XorTag.Commands;

namespace XorTag.Controllers;

[ApiController]
public class StatsController: ControllerBase
{
    private readonly StatsCommand statsCommand;

    public StatsController(StatsCommand statsCommand)
    {
        this.statsCommand = statsCommand;
    }

    [Route("/stats")]
    [OutputCache(Duration = 1, VaryByQueryKeys = [])]
    public StatsResult Get()
    {
        return statsCommand.Execute();
    }
}