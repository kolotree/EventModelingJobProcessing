using System;
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
        
        private static readonly DateTime MachineStoppedTimestamp = DateTime.UtcNow;
        private static readonly DateTime MachineStartedTimestamp = MachineStoppedTimestamp.AddSeconds(10);

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
                    LastStoppedAt = MachineStoppedTimestamp,
                    StartedAt = MachineStartedTimestamp
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
                    LastStoppedAt = MachineStoppedTimestamp,
                    StartedAt = MachineStartedTimestamp
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: MachineId"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_last_stopped_at_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    StartedAt = MachineStartedTimestamp
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: LastStoppedAt"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_started_at_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    LastStoppedAt = MachineStoppedTimestamp,
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: StartedAt"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_stoppage_is_not_found()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    LastStoppedAt = MachineStoppedTimestamp,
                    StartedAt = MachineStartedTimestamp
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Stoppage doesn't exist."));
        }
        
        [Fact]
        public async Task success_returned_with_produced_machine_started_event_if_stoppage_is_present()
        {
            _store.Given(
                $"MachineStoppage-AlingConel|Machine1|{MachineStoppedTimestamp.Ticks}",
                new MachineStopped("AlingConel", "Machine1", MachineStoppedTimestamp).ToEventEnvelope());
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    LastStoppedAt = MachineStoppedTimestamp,
                    StartedAt = MachineStartedTimestamp
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.Success);
            _store.ProducedEventEnvelopes.Should().Contain(new MachineStarted("AlingConel", "Machine1", MachineStoppedTimestamp, MachineStartedTimestamp).ToEventEnvelope());
        }
        
        [Fact]
        public async Task success_returned_without_produced_machine_started_event_if_machine_is_already_started()
        {
            _store.Given(
                $"MachineStoppage-AlingConel|Machine1|{MachineStoppedTimestamp.Ticks}",
                new MachineStopped("AlingConel", "Machine1", MachineStoppedTimestamp).ToEventEnvelope());
            _store.Given(
                $"MachineStoppage-AlingConel|Machine1|{MachineStoppedTimestamp.Ticks}",
                new MachineStarted("AlingConel", "Machine1", MachineStoppedTimestamp, MachineStartedTimestamp).ToEventEnvelope());
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    LastStoppedAt = MachineStoppedTimestamp,
                    StartedAt = MachineStartedTimestamp
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.Success);
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
    }
}