﻿using System;

namespace JobProcessing.Abstractions
{
    public readonly struct GlobalPosition : IComparable<GlobalPosition>
    {
        public ulong Part1 { get; }
        public ulong Part2 { get; }

        public GlobalPosition(ulong part1, ulong part2)
        {
            Part1 = part1;
            Part2 = part2;
        }

        public static readonly GlobalPosition Start = new(0UL, 0UL);
        
        public static GlobalPosition Of(ulong part1, ulong part2 = 0L) => new(part1, part2);

        public static GlobalPosition? Max(GlobalPosition? a, GlobalPosition? b)
        {
            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            return a.Value > b.Value ? a : b;
        }
        
        public static bool operator <(GlobalPosition p1, GlobalPosition p2)
        {
            return p1.Part1 < p2.Part1 || (p1.Part1 == p2.Part1 && p1.Part2 < p2.Part2);
        }
        
        public static bool operator >(GlobalPosition p1, GlobalPosition p2)
        {
            return p1.Part1 > p2.Part1 || (p1.Part1 == p2.Part1 && p1.Part2 > p2.Part2);
        }
        
        public static bool operator >=(GlobalPosition p1, GlobalPosition p2) => p1 > p2 || p1 == p2;
        
        public static bool operator <=(GlobalPosition p1, GlobalPosition p2) => p1 < p2 || p1 == p2;
        
        public static bool operator ==(GlobalPosition p1, GlobalPosition p2) => 
            p1.Part1 == p2.Part1 && p1.Part2 == p2.Part2;
        
        public static bool operator !=(GlobalPosition p1, GlobalPosition p2) => !(p1 == p2);
        
        public override bool Equals(object obj) => 
            obj is GlobalPosition && Equals((GlobalPosition)obj);

        public bool Equals(GlobalPosition other) => this == other;

        public override int GetHashCode()
        {
            unchecked
            {
                return (Part1.GetHashCode()*397) ^ Part2.GetHashCode();
            }
        }
        
        public override string ToString() => $"P1:{Part1}/P2:{Part2}";

        public int CompareTo(GlobalPosition other)
        {
            var part1Comparison = Part1.CompareTo(other.Part1);
            if (part1Comparison != 0) return part1Comparison;
            return Part2.CompareTo(other.Part2);
        }
    }
}