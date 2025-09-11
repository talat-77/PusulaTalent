FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY SchoolManangement.API/*.csproj ./SchoolManangement.API/
COPY SchoolManangement.Business/*.csproj ./SchoolManangement.Business/
COPY SchoolManangement.DataAccess/*.csproj ./SchoolManangement.DataAccess/
COPY SchoolManangement.Entity/*.csproj ./SchoolManangement.Entity/

RUN dotnet restore

COPY . .

WORKDIR /src/SchoolManangement.API
RUN dotnet publish -c Release -o /app/

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/ .

EXPOSE $PORT
ENV ASPNETCORE_URLS=http://*:$PORT

ENTRYPOINT ["dotnet", "SchoolManangement.API.dll"]