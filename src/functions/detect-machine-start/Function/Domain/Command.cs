﻿using System;
using JobProcessing.Abstractions;

namespace Function.Domain
{
    public sealed class Command : ICommand
    {
        public CommandMetadata Metadata { get; }
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime LastStoppedAt  { get; }
        public DateTime StartedAt { get; }


        public Command(
            CommandMetadata metadata,
            string? factoryId,
            string? machineId,
            DateTime? lastStoppedAt,
            DateTime? startedAt)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(Metadata));
            FactoryId = factoryId ?? throw new ArgumentNullException(nameof(FactoryId));
            MachineId = machineId ?? throw new ArgumentNullException(nameof(MachineId));
            LastStoppedAt = lastStoppedAt ?? throw new ArgumentNullException(nameof(LastStoppedAt));
            StartedAt = startedAt ?? throw new ArgumentNullException(nameof(StartedAt));
            if (StartedAt < LastStoppedAt) throw new ArgumentNullException(nameof(StartedAt));
        }

        internal StreamId StoppageId => StreamId.AssembleFor<MachineStoppage>(
            FactoryId,
            MachineId,
            LastStoppedAt.Ticks.ToString());

        internal MachineStarted ToMachineStarted() => new(
            FactoryId,
            MachineId,
            LastStoppedAt,
            StartedAt);
    }
}