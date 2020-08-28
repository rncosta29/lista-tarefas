#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["lista_tarefas_api.csproj", ""]
RUN dotnet restore "./lista_tarefas_api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "lista_tarefas_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lista_tarefas_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN useradd -m myappuser
USER myappuser

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet lista_tarefas_api.dll