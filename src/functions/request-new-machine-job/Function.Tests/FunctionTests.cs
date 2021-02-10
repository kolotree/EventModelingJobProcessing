using System.Linq;
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

        public FunctionTests()
        {
            _functionHandler = new(_store, StubDateTimeProvider.Today);
        }
        
        [Fact]
        public async Task error_returned_if_factory_id_is_not_passed()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    MachineId = "machine1",
                    JobStartTime = StubDateTimeProvider.Today.CurrentUtcDateTime
                    
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: factoryId"));
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task error_returned_if_machine_id_is_not_passed()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    JobStartTime = StubDateTimeProvider.Today.CurrentUtcDateTime
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: machineId"));
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task error_returned_if_job_start_time_is_not_passed()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "machine1"
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: jobStartTime"));
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task error_returned_if_job_start_time_is_from_future()
        {
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "machine1",
                    JobStartTime = StubDateTimeProvider.Today.CurrentUtcDateTime.AddSeconds(1)
                }.ToHttpRequest());

            functionResult.Should().Be(FunctionResult.BadRequestFailureWith("Invalid input: JobStartTime"));
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task failure_returned_if_stream_with_same_id_exists()
        {
            _store.Given($"NewMachineJobRequest-AlingConel|machine1|{StubDateTimeProvider.Today.CurrentUtcDateTime.Ticks}", new NewMachineJobRequested("AlingConel", "machine1", "SomeJobId", StubDateTimeProvider.Today.CurrentUtcDateTime).ToEventEnvelope());
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "machine1",
                    JobStartTime = StubDateTimeProvider.Today.CurrentUtcDateTime
                    
                }.ToHttpRequest());

            functionResult.IsSuccess.Should().BeFalse();
            functionResult.Error.Should().Be("Item with the same ID already in store");
            _store.ProducedEventEnvelopes.Should().BeEmpty();
        }
        
        [Fact]
        public async Task success_returned_if_stream_with_same_id_doesnt_exist()
        {
            _store.Given($"NewMachineJobRequest-AlingConel|machine1|{StubDateTimeProvider.Yesterday.CurrentUtcDateTime.Ticks}", new NewMachineJobRequested("AlingConel", "machine1", "SomeJobId", StubDateTimeProvider.Yesterday.CurrentUtcDateTime).ToEventEnvelope());
            
            var functionResult = await  _functionHandler.Handle(
                new
                {
                    FactoryId = "AlingConel",
                    MachineId = "machine1",
                    JobStartTime = StubDateTimeProvider.Today.CurrentUtcDateTime
                    
                }.ToHttpRequest());

            functionResult.IsSuccess.Should().BeTrue();
            _store.ProducedEventEnvelopes.Select(ee => ee.Type).Should().Contain(nameof(NewMachineJobRequested));
        }
    }
}