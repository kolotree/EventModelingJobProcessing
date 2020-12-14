#!/bin/sh

echo $(docker-compose -f docker-compose-infrastructure.yml down)
echo $(docker-compose -f docker-compose-infrastructure.yml up -d)
sleep 2
echo $($(dirname $0)/projections/create-all-projections.sh)