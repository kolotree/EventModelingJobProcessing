fromAll()
.partitionBy(function(e){
	return e.body.FactoryId;
})
.when({
    $init: function(){
        return {}
    },
    NewMachineJobStarted: function(s,e){
        s[e.body.JobId] = e.body.MachineId;
    },
    MachineJobCompleted: function(s,e){
        delete s[e.body.JobId];
    }
})