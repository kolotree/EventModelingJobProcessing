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
        [Route(nameof(DetectMachineStop))]
        public async Task<IActionResult> DetectMachineStop([FromBody] DetectMachineStopDto commandDto)
        {
            await _externalCommandHandlers.Handle(commandDto.ToCommand());
            return Ok();
        }
        
        [HttpPost]
        [Route(nameof(DetectMachineStart))]
        public async Task<IActionResult> DetectMachineStart([FromBody] DetectMachineStartDto commandDto)
        {
            await _externalCommandHandlers.Handle(commandDto.ToCommand());
            return Ok();
        }
        
        [HttpPost]
        [Route(nameof(CompleteMachineJob))]
        public async Task<IActionResult> CompleteMachineJob([FromBody] CompleteMachineJob commandDto)
        {
            await _externalCommandHandlers.Handle(commandDto.ToCommand());
            return Ok();
        }
    }
}