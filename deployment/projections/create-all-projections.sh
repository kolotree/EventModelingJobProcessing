#!/bin/sh

ES_URL="http://localhost:2113"
ES_CRED="admin:changeit"

MACHINE_JOB_PROCESSOR_VIEW_PROJECTION_NAME="MachineJobProcessorView"
MACHINE_JOB_PROCESSOR_VIEW_PROJECTION_PROJECTION_PATH="@../CommandSide/Processors/MachineJobProcessor/Domain/MachineJobProcessorViewProjection.js"

echo $($(dirname $0)/create-projection.sh $ES_URL $ES_CRED $MACHINE_JOB_PROCESSOR_VIEW_PROJECTION_NAME $MACHINE_JOB_PROCESSOR_VIEW_PROJECTION_PROJECTION_PATH)
