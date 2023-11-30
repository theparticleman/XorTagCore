using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using XorTag.Domain;

namespace XorTag.Controllers;

[ApiController]
public class MapController : ControllerBase
{
    private readonly MapImageBuilder mapImageBuilder;
    private readonly ILogger<MapController> logger;

    public MapController(MapImageBuilder mapImageBuilder, ILogger<MapController> logger)
    {
        this.mapImageBuilder = mapImageBuilder;
        this.logger = logger;
    }

    [Route("/map")]
    [OutputCache(Duration = 1, VaryByQueryKeys = [])]
    public IActionResult Get()
    {
        return File(mapImageBuilder.BuildImage(), "image/png");
    }
}