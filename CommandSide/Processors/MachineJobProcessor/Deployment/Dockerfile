FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

WORKDIR /app/CommandSide/Processors/MachineJobProcessor
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app

COPY --from=build /app/CommandSide/Processors/MachineJobProcessor/out /app/

RUN sh /app/Deployment/CreateProjection.sh

ENTRYPOINT ["dotnet", "MachineJobProcessor.dll"]
