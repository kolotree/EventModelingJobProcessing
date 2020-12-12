using System.Threading.Tasks;
using CommandsWireUp;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Commands : ControllerBase
    {
        private readonly ExternalCommandHandlers _externalCommandHandlers;
        
        public Commands(ExternalCommandHandlers externalCommandHandlers)
        {
            _externalCommandHandlers = externalCommandHandlers;
        }
        
        [HttpPost]
        [Route(nameof(CreateMachine))]
        public async Task<IActionResult> CreateMachine([FromBody] CreateMachineDto commandDto)
        {
            await _externalCommandHandlers.Handle(commandDto.ToCommand());
            return Ok();
        }

        [HttpPost]
        [Route(nameof(DetectMachineStop))]
        public async Task<IActionResult> DetectMachineStop([FromBody] DetectMachineStopDto commandDto)
        {
            await _externalCommandHandlers.Handle(commandDto.ToCommand());
            return Ok();
        }
    }
}