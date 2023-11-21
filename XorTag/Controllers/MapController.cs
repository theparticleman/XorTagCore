using Microsoft.AspNetCore.Mvc;
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
    [ResponseCache(Duration = 1)]
    public IActionResult Get()
    {
        return File(mapImageBuilder.BuildImage(), "image/png");
    }
}