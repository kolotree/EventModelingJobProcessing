fromAll()
.partitionBy(function(e){
	return e.body.FactoryId;
})
.when({
    $init: function(){
        return {}
    },
    MachineStopped: function(s,e){
        s[`${e.body.MachineId}|${e.body.StoppedAt}`] = true;
    },
    MachineStarted: function(s,e){
        delete s[`${e.body.MachineId}|${e.body.LastStoppedAt}`];
    }
})