using JobProcessing.Abstractions;
using ViewStore.Abstractions;

namespace Processor
{
    internal static class GlobalVersionExtensions
    {
        public static GlobalVersion ToGlobalVersion(this GlobalPosition position) =>
            GlobalVersion.Of((long)position.Part1, (long)position.Part2);

        public static GlobalPosition ToGlobalPosition(this GlobalVersion globalVersion) => 
            GlobalPosition.Of((ulong)globalVersion.Part1, (ulong)globalVersion.Part2);
    }
}