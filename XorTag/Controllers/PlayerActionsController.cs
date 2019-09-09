using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers
{
    [ApiController]
    public class PlayerActionsController: ControllerBase
    {
        private readonly RegisterPlayerCommand registerPlayerCommand;
        private readonly MovePlayerCommand movePlayerCommand;

        public PlayerActionsController(RegisterPlayerCommand registerPlayerCommand, MovePlayerCommand movePlayerCommand)
        {
            this.registerPlayerCommand = registerPlayerCommand;
            this.movePlayerCommand = movePlayerCommand;
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
    }
}