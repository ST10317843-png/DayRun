FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DayRun.Web/DayRun.Web.csproj", "DayRun.Web/"]
RUN dotnet restore "DayRun.Web/DayRun.Web.csproj"
COPY . .
WORKDIR "/src/DayRun.Web"
RUN dotnet build "DayRun.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DayRun.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DayRun.Web.dll"]