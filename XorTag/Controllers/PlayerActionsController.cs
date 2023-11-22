using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers;

[ApiController]
public class PlayerActionsController: ControllerBase
{
    private readonly RegisterPlayerCommand registerPlayerCommand;
    private readonly MovePlayerCommand movePlayerCommand;
    private readonly LookCommand lookCommand;

    public PlayerActionsController(RegisterPlayerCommand registerPlayerCommand, MovePlayerCommand movePlayerCommand, LookCommand lookCommand)
    {
        this.registerPlayerCommand = registerPlayerCommand;
        this.movePlayerCommand = movePlayerCommand;
        this.lookCommand = lookCommand;
    }

    [Route("/register")]
    public CommandResult Register()
    {
        return registerPlayerCommand.Execute();
    }

    [Route("/move{direction}/{playerId}")]
    public CommandResult Move(string direction, int playerId)
    {
        return movePlayerCommand.Execute(direction, playerId);
    }

    [Route("/look/{playerId}")]
    public CommandResult Look(int playerId)
    {
        return lookCommand.Execute(playerId);
    }
}