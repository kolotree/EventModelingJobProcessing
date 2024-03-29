options({
    resultStreamName: "MachineJobProcessorView"
})
fromAll()
.partitionBy(function(e){
	return e.body.FactoryId + "|" + e.body.MachineId;
})
.when({
    $init: function(){
        return {
			FactoryId: null,
			MachineId: null,
			MachineStartedTime: null,
			JobId: null,
            RequestedJobTime: null
        }
    },
    MachineStopped: function(s,e){
		s.FactoryId = e.body.FactoryId;
		s.MachineId = e.body.MachineId;
		s.MachineStartedTime = null;
        s.LastAppliedEventType = e.eventType;
        s.LastAppliedEventId = e.metadata.$eventId;
        s.LastAppliedEventCorrelationId = e.metadata.$correlationId;
    },
    MachineStarted: function(s,e){
        s.FactoryId = e.body.FactoryId;
		s.MachineId = e.body.MachineId;
		s.MachineStartedTime = e.body.StartedAt;
        s.LastAppliedEventType = e.eventType;
        s.LastAppliedEventId = e.metadata.$eventId;
        s.LastAppliedEventCorrelationId = e.metadata.$correlationId;
    },
    NewMachineJobStarted: function(s,e){
        s.FactoryId = e.body.FactoryId;
        s.MachineId = e.body.MachineId;
		s.JobId = e.body.JobId;
		s.RequestedJobTime = null;
        s.LastAppliedEventType = e.eventType;
        s.LastAppliedEventId = e.metadata.$eventId;
        s.LastAppliedEventCorrelationId = e.metadata.$correlationId;
    },
    MachineJobCompleted: function(s,e){
        s.FactoryId = e.body.FactoryId;
        s.MachineId = e.body.MachineId;
        s.JobId = null;
        s.RequestedJobTime = null;
        s.LastAppliedEventType = e.eventType;
        s.LastAppliedEventId = e.metadata.$eventId;
        s.LastAppliedEventCorrelationId = e.metadata.$correlationId;
    },
    NewMachineJobRequested: function (s,e){
        s.FactoryId = e.body.FactoryId;
        s.MachineId = e.body.MachineId;
        s.RequestedJobTime = e.body.RequestedJobTime;
        s.LastAppliedEventType = e.eventType;
        s.LastAppliedEventId = e.metadata.$eventId;
        s.LastAppliedEventCorrelationId = e.metadata.$correlationId;
    }
})
.outputState();