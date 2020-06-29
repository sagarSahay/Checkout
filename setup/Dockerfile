FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

ARG version=0.0.0

COPY ./src/PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj ./src/PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj
RUN dotnet restore ./src/PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj

COPY ./src/PaymentGateway.Commands/PaymentGateway.Commands.csproj ./src/PaymentGateway.Commands/PaymentGateway.Commands.csproj
RUN dotnet restore ./src/PaymentGateway.Commands/PaymentGateway.Commands.csproj

COPY ./src/PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj ./src/PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj
RUN dotnet restore ./src/PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj

COPY ./src/PaymentGateway.WriteModel.Application/PaymentGateway.WriteModel.Application.csproj ./src/PaymentGateway.WriteModel.Application/PaymentGateway.WriteModel.Application.csproj
RUN dotnet restore ./src/PaymentGateway.WriteModel.Application/PaymentGateway.WriteModel.Application.csproj

COPY ./Tests/PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj ./Tests/PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj
RUN dotnet restore ./Tests/PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj

COPY . .

RUN dotnet publish -c Release /p:Version=$version ./src/PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj
RUN dotnet publish -c Release /p:Version=$version ./src/PaymentGateway.WriteModel.Application/PaymentGateway.WriteModel.Application.csproj

RUN dotnet test -c Release ./Tests/PaymentGateway.WriteModel.API.Tests/PaymentGateway.WriteModel.API.Tests.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

EXPOSE 9010

COPY --from=build-env /app/PaymentGateway.WriteModel.API/bin/Release/netcoreapp3.1/publish .

# Using the exec form of CMD allows SIGINT to be passed from container to app
CMD ["dotnet", "PaymentGateway.WriteModel.API.dll"]

