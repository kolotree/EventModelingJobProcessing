#!/bin/sh

EVENT_STORE_URL=$1
EVENT_STORE_CREDENTIALS=$2
PROJECTION_NAME=$3
PROJECTION_PATH=$4

URL=$EVENT_STORE_URL/projections/continuous?name=$PROJECTION_NAME%26type=js%26enabled=true%26emit=true%26trackemittedstreams=true

STATUS=$(curl -i --data-binary $PROJECTION_PATH $URL -u $EVENT_STORE_CREDENTIALS)

if [[ "${STATUS}" == *"HTTP/1.1 201"* ]]; then
  echo "[SUCCESS] Projection created."
  exit 0
fi

if [[ "${STATUS}" == *"HTTP/1.1 409"* ]]; then
  echo "[SUCCESS] Projection already created."
  exit 0
fi

>&2 echo "error" "[ERROR] Projection was not created."
echo "${STATUS}"
exit 1