FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR client-p2/
COPY . .
RUN dotnet build
RUN dotnet publish -c Release -o out BookMark.Client/BookMark.Client.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR client-dist/
COPY --from=build client-p2/out/ ./
CMD [ "dotnet", "BookMark.Client.dll" ]
