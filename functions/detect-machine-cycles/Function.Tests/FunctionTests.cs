using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Function.Domain;
using JobProcessing.Infrastructure.Serialization;
using JobProcessing.InMemoryStore;
using Xunit;

namespace Function.Tests
{
    public sealed class FunctionTests
    {
        private readonly InMemoryStore _store = new();
        private readonly FunctionHandler _functionHandler;

        private static readonly DateTime Cycle1Timestamp = DateTime.UtcNow;
        private static readonly DateTime Cycle2Timestamp = Cycle1Timestamp.AddSeconds(10);

        public FunctionTests()
        {
            _functionHandler = new(_store);
        }
        
        [Fact]
        public async Task bad_request_returned_if_factory_id_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    MachineId = "Machine1",
                    Timestamps = new [] { Cycle1Timestamp, Cycle2Timestamp }
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: FactoryId"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_machine_id_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    Timestamps = new [] { Cycle1Timestamp, Cycle2Timestamp }
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: MachineId"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_timestamps_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: Timestamps"));
        }
        
        [Fact]
        public async Task success_returned_without_produced_event_when_zero_cycles_are_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    Timestamps = Array.Empty<DateTime>()
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.Success);
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task success_returned_with_produced_event_when_some_cycles_are_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    Timestamps = new [] { Cycle1Timestamp, Cycle2Timestamp }
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.Success);
            _store.ProducedEventEnvelopes.Should().Contain(new MachineCyclesDetected("AlingConel", "Machine1", new List<DateTime> {Cycle1Timestamp, Cycle2Timestamp}).ToEventEnvelope());
        }
        
        [Fact]
        public async Task success_returned_with_produced_event_even_when_same_cycles_are_already_present()
        {
            _store.Given("MachineCycles-AlingConel|Machine1", new MachineCyclesDetected("AlingConel", "Machine1", new List<DateTime> {Cycle1Timestamp, Cycle2Timestamp}).ToEventEnvelope());
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    Timestamps = new [] { Cycle1Timestamp, Cycle2Timestamp }
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.Success);
            _store.ProducedEventEnvelopes.Should().Contain(new MachineCyclesDetected("AlingConel", "Machine1", new List<DateTime> {Cycle1Timestamp, Cycle2Timestamp}).ToEventEnvelope());
        }
    }
}