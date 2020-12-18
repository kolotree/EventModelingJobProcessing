FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

WORKDIR /app/CommandSide/UiCommands/WebApplication
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

COPY --from=build /app/CommandSide/UiCommands/WebApplication/out /app/

ENTRYPOINT ["dotnet", "WebApplication.dll"]
