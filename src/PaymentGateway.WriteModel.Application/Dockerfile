FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish PaymentGateway.WriteModel.Application/PaymentGateway.WriteModel.Application.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PaymentGateway.WriteModel.Application.dll"]