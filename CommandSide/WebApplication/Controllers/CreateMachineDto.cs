using CreateMachine;

namespace WebApplication.Controllers
{
    public sealed class CreateMachineDto
    {
        public string FactoryId { get; set; }
        public string RemoteId { get; set; }
        public string MachineId { get; set; }
        
        internal CreateMachineCommand ToCommand() => new CreateMachineCommand(
            FactoryId,
            RemoteId,
            MachineId);
    }
}