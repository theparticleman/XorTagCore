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

    [Route("duplicate-ids-exist")]
    public bool HasDuplicateIds()
    {
        var groupedById = playerRepository.GetAllPlayers().GroupBy(x => x.Id);
        var groupsWithMoreThanOne = groupedById.Where(x => x.Count() > 1);
        return groupsWithMoreThanOne.Any();
    }

    [Route("duplicate-names-exist")]
    public bool HasDuplicateNames()
    {
        var groupedByName = playerRepository.GetAllPlayers().GroupBy(x => x.Name);
        var groupsWithhMoreThanOne = groupedByName.Where(x => x.Count() > 1);
        return groupsWithhMoreThanOne.Any();
    }

}