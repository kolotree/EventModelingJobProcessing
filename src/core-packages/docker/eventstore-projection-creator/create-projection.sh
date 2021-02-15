#!/bin/bash

if [[ -z "${EventStore_ConnectionString}" ]]; then
  echo "EventStore_ConnectionString environment variable is not set."
  exit 1
fi

if [[ -z "${EventStore_Credentials}" ]]; then
  echo "EventStore_Credentials environment variable is not set."
  exit 1
fi

if [[ -z "${Projection_Name}" ]]; then
  echo "Projection_Name environment variable is not set."
  exit 1
fi

RESULT=$(curl -k -i --data-binary  "@./projection.js" "${EventStore_ConnectionString}/projections/continuous?name=${Projection_Name}&type=js&enabled=true&emit=true&trackemittedstreams=true" -u "${EventStore_Credentials}")

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
