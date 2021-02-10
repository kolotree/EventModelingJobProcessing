#!/bin/bash

ES_URL=https://node1.eventstore:2113
ES_CRED=admin:changeit

bash ./create-projection.sh $ES_URL $ES_CRED MachineJobProcessorView "@../../processors/MachineJobProcessor/Domain/MachineJobProcessorViewProjection.js"
