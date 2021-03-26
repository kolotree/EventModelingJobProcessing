FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app

COPY . ./
COPY ./Nuget.Config /root/.nuget/NuGet/NuGet.Config
RUN dotnet restore
RUN dotnet test
WORKDIR /app/Processor
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine AS runtime
WORKDIR /app

COPY --from=build /app/Processor/out /app/

ENTRYPOINT ["dotnet", "Processor.dll"]
