FROM ghcr.io/kolotree/eventstore-projection-creator:1.0

WORKDIR /projection

COPY ./projection.js ./projection.js

ENTRYPOINT ["bash", "./create-projection.sh"]
