fromAll()
.partitionBy(function(e){
	return e.body.FactoryId;
})
.when({
    $init: function(){
        return {}
    },
    MachineStopped: function(s,e){
        s[e.body.StoppedAt] = e.body.MachineId;
    },
    MachineStarted: function(s,e){
        delete s[e.body.LastStoppedAt];
    }
})