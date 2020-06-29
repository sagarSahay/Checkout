FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

ARG version=0.0.0

COPY ./PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj ./PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj
RUN dotnet restore ./PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj

COPY ./PaymentGateway.Commands/PaymentGateway.Commands.csproj ./PaymentGateway.Commands/PaymentGateway.Commands.csproj
RUN dotnet restore ./PaymentGateway.Commands/PaymentGateway.Commands.csproj

COPY ./PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj ./PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj
RUN dotnet restore ./PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj

COPY ./PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj ./PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj
RUN dotnet restore ./PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj

COPY . .

RUN dotnet publish -c Release /p:Version=$version ./PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj

RUN dotnet test -c Release ./PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

EXPOSE 9010

COPY --from=build-env /app/PaymentGateway.WriteModel.API/bin/Release/netcoreapp3.1/publish .

# Using the exec form of CMD allows SIGINT to be passed from container to app
CMD ["dotnet", "PaymentGateway.WriteModel.API.dll"]

