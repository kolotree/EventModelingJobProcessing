options({
    resultStreamName: "MachineJobProcessingViewStream"
})
fromAll()
.partitionBy(function(e){
	return e.body.FactoryId;
})
.when({
    $init: function(){
        return {
            table: new Map(),
            lastAppliedEventType: null,
            lastAffectedMachineId: null
        }
    },
    MachineCreated: function(s,e){
        s.table[e.body.MachineId] = {};
        s.lastAppliedEventType = e.eventType;
        s.lastAffectedMachineId = e.body.MachineId;
    },
    MachineStopped: function(s,e){
        s.table[e.body.MachineId].MachineStatus = "Stopped";
        s.lastAppliedEventType = e.eventType;
        s.lastAffectedMachineId = e.body.MachineId;
    },
    MachineStarted: function(s,e){
        s.table[e.body.MachineId].MachineStatus = "Started";
        s.lastAppliedEventType = e.eventType;
        s.lastAffectedMachineId = e.body.MachineId;
    },
    NewJobStarted: function(s,e){
        row = s.table[e.body.MachineId];
        row.JobId = e.JobId;
        row.JobState = "Started";
        s.lastAppliedEventType = e.eventType;
        s.lastAffectedMachineId = e.body.MachineId;
    },
    JobCompleted: function(s,e){
        row = s.table[e.body.MachineId];
        row.JobState = "Completed";
        s.lastAppliedEventType = e.eventType;
        s.lastAffectedMachineId = e.body.MachineId;
    }
})
.outputState();