FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY . ./
RUN echo $(ls -a)
RUN dotnet publish PaymentGateway.ReadModel.Denormalizer/PaymentGateway.ReadModel.Denormalizer.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PaymentGateway.ReadModel.Denormalizer.dll"]