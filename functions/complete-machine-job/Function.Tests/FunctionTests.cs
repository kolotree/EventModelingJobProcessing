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

        private static readonly DateTime Job1StartedTime = DateTime.UtcNow;
        private static readonly string Job1Id = Job1StartedTime.Ticks.ToString();

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
                    JobId = Job1Id
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: factoryId"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_machine_id_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    JobId = Job1Id
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: machineId"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_job_id_is_not_provided()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1"
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: jobId"));
        }
        
        [Fact]
        public async Task bad_request_returned_if_stream_doesnt_exist()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    JobId = Job1Id
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Job doesn't exist."));
        }
        
        [Fact]
        public async Task success_returned_with_machine_job_completed_event_when_started_job_exists()
        {
            _store.Given($"MachineJob-AlingConel|Machine1|{Job1Id}", new NewMachineJobStarted("AlingConel", "Machine1", Job1Id, Job1StartedTime).ToEventEnvelope());
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "Machine1",
                    JobId = Job1Id
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.Success);
            _store.ProducedEventEnvelopes.Should().Contain(new MachineJobCompleted("AlingConel", "Machine1", Job1Id).ToEventEnvelope());
        }
    }
}