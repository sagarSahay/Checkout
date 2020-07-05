FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 5009
ENV ASPNETCORE_URLS=http://*:5009

COPY . ./
RUN dotnet publish PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PaymentGateway.WriteModel.API.dll"]