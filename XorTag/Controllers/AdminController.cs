using Microsoft.AspNetCore.Mvc;
using XorTag.Domain;

namespace XorTag.Controllers;

[ApiController]
[Route("/admin")]
public class AdminController : ControllerBase
{
    private readonly IPlayerRepository playerRepository;

    public AdminController(IPlayerRepository playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    [Route("clearall")]
    public void ClearAll()
    {
        playerRepository.ClearAllPlayers();
    }
}