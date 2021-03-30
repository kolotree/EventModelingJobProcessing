﻿using System;
using System.Collections.Generic;
using JobProcessing.Abstractions;

namespace Projection.Domain
{
    public sealed class MachineStopped : ValueObject, IEvent
    {
        public string FactoryId { get; }
        public string MachineId { get; }
        public DateTime StoppedAt { get; }
        public string ViewId => $"{FactoryId}|{MachineId}";

        public MachineStopped(
            string factoryId,
            string machineId,
            DateTime stoppedAt)
        {
            FactoryId = factoryId;
            MachineId = machineId;
            StoppedAt = stoppedAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FactoryId;
            yield return MachineId;
            yield return StoppedAt;
        }
    }
}