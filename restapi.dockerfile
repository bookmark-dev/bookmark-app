FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR restapi-p2/
COPY . .
RUN dotnet build
RUN dotnet publish -c Release -o out BookMark.RestApi/BookMark.RestApi.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR restapi-dist/
COPY --from=build restapi-p2/out/ ./
CMD [ "dotnet", "BookMark.RestApi.dll" ]
