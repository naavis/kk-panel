#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KomakallioPanel/KomakallioPanel.csproj", "KomakallioPanel/"]
RUN dotnet restore "KomakallioPanel/KomakallioPanel.csproj"
COPY . .
WORKDIR "/src/KomakallioPanel"
RUN dotnet build "KomakallioPanel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KomakallioPanel.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KomakallioPanel.dll"]