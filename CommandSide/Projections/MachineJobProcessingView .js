options({
    resultStreamName: "MachineJobProcessingView"
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
			JobId: null
        }
    },
    MachineStopped: function(s,e){
		s.FactoryId = e.body.FactoryId;
		s.MachineId = e.body.MachineId;
		s.MachineStartedTime = null;
        s.LastAppliedEventType = e.eventType;
    },
    MachineStarted: function(s,e){
        s.FactoryId = e.body.FactoryId;
		s.MachineId = e.body.MachineId;
		s.MachineStartedTime = e.body.StartedAt;
        s.LastAppliedEventType = e.eventType;
    },
    NewMachineJobStarted: function(s,e){
		s.FactoryId = e.body.FactoryId;
		s.MachineId = e.body.MachineId;
		s.JobId = e.body.JobId;
        s.LastAppliedEventType = e.eventType;
    },
    JobCompleted: function(s,e){
        s.FactoryId = e.body.FactoryId;
		s.MachineId = e.body.MachineId;
		s.JobId = null;
        s.LastAppliedEventType = e.eventType;
    }
})
.outputState();