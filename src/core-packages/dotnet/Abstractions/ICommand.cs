namespace JobProcessing.Abstractions
{
    public interface ICommand
    {
        CommandMetadata Metadata { get; }
    }
}