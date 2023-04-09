FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine  AS build
WORKDIR /buildapp
COPY . . 
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine AS finalimage
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AppMultiContainers.dll"]