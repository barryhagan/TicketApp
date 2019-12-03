FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app
COPY TicketApp.sln ./
COPY src/. ./src/
WORKDIR /app/src/TicketApi
RUN dotnet publish -c Release -o out

FROM node:13.2 as node
WORKDIR /app
COPY --from=build /app/src/TicketApi/ReactApp ./
RUN npm i
RUN npm run-script build

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/src/TicketApi/out ./
COPY --from=node /app/build ./ReactApp/build
ENTRYPOINT ["dotnet", "TicketApi.dll"]