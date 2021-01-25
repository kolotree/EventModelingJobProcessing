#!/bin/bash

EVENT_STORE_URL=$1
EVENT_STORE_CREDENTIALS=$2
PROJECTION_NAME=$3
PROJECTION_PATH=$4

RESULT=$(curl -k -i --data-binary  ${PROJECTION_PATH} "${EVENT_STORE_URL}/projections/continuous?name=${PROJECTION_NAME}&type=js&enabled=true&emit=true&trackemittedstreams=true" -u "${EVENT_STORE_CREDENTIALS}")

if [[ ${RESULT} == *"HTTP/2 201"* ]]; then
  echo "[SUCCESS] Projection created."
  exit 0
fi

if [[ ${RESULT} == *"HTTP/2 409"* ]]; then
  echo "[SUCCESS] Projection already created."
  exit 0
fi

>&2 echo "error" "[ERROR] Projection was not created."
echo "${RESULT}"
exit 1