using JobProcessing.Abstractions;
using ViewStore.Abstractions;

namespace Projection
{
    internal static class GlobalVersionExtensions
    {
        public static GlobalVersion ToGlobalVersion(this GlobalPosition position) =>
            GlobalVersion.FromUlong(position.Part1, position.Part2);

        public static GlobalPosition ToGlobalPosition(this GlobalVersion globalVersion)
        {
            var (part1, part2) = globalVersion.ToUlong();
            return GlobalPosition.Of(part1, part2);
        }
    }
}