options({
    resultStreamName: "MachineJobProcessingViewStream"
})
fromAll()
.partitionBy(function(e){
	return e.body.FactoryId + "|" + e.body.MachineId;
})
.when({
    $init: function(){
        return {
            MachineState: null,
            JobId: null,
            JobState: null,
            LastAppliedEventType: null
        }
    },
    MachineStopped: function(s,e){
        s.MachineState = "Stopped";
        s.LastAppliedEventType = e.eventType;
    },
    MachineStarted: function(s,e){
        s.MachineState = "Started";
        s.LastAppliedEventType = e.eventType;
    },
    NewJobStarted: function(s,e){
        s.JobId = e.JobId;
        s.JobState = "Started";
        s.lastAppliedEventType = e.eventType;
    },
    JobCompleted: function(s,e){
        s.JobId = e.JobId;
        s.JobState = "Completed";
        s.lastAppliedEventType = e.eventType;
    }
})
.outputState();