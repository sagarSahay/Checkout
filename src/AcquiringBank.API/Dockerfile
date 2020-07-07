FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 8080
EXPOSE 6002
ENV ASPNETCORE_URLS=http://*:6002

COPY . ./
RUN echo $(ls -a)
RUN dotnet publish AcquiringBank.API/AcquiringBank.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "AcquiringBank.API.dll"]