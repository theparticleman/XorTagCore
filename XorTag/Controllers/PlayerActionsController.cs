using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers
{
    [ApiController]
    public class PlayerActionsController: ControllerBase
    {
        private readonly RegisterPlayerCommand registerPlayerCommand;

        public PlayerActionsController(RegisterPlayerCommand registerPlayerCommand)
        {
            this.registerPlayerCommand = registerPlayerCommand;
        }

        [Route("/register")]
        public CommandResult Register()
        {
            return registerPlayerCommand.Execute();
        }
    }
}