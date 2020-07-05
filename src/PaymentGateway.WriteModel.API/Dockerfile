FROM microsoft/aspnetcore AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build AS build
WORKDIR /src
COPY CheckoutPaymentGateway.sln ./
COPY src/PaymentGateway.WriteModel.API/PaymentGateway.WriteModel.API.csproj src/PaymentGateway.WriteModel.API/
COPY src/PaymentGateway.Commands/PaymentGateway.Commands.csproj src/PaymentGateway.Commands/
COPY src/PaymentGateway.Messages.Common/PaymentGateway.Messages.Common.csproj src/PaymentGateway.Messages.Common/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/src/PaymentGateway.WriteModel.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentGateway.WriteModel.API.dll"]
