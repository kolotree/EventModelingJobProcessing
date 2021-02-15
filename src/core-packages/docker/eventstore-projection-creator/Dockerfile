FROM alpine:3.13.1
RUN apk --no-cache add curl
RUN apk add --no-cache bash

WORKDIR /projection

COPY ./create-projection.sh ./create-projection.sh
