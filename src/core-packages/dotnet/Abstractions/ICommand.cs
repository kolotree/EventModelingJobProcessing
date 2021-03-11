namespace JobProcessing.Abstractions
{
    public interface ICommand
    {
        string Id { get; }
        string CorrelationId { get; }
    }
}