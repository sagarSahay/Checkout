# Checkout payment gateway

## Overview

-This payment gateway accepts a request from merchant and then calls an acquiring bank api to process the payment.
-The acquiring bank send back a payment response id and a status message. 
-The merchant can then quey the payment gateway to get the result of the payment and all the transactions done under its Id.

## My assumptions
- Each merchant is registered with a bank
- In this solution 'merchant1' is registered with LLoyds and ever other merchant id is registered with Barclays.

## Solution
- My solution is based on CQRS architecture and has following executables:
1) PaymentGateway.WriteModel.API 
2) PaymentGateway.WriteModel.Application
3) PaymentGateway.ReadModel.Denormalizer
4) PaymentGateway.ReadModel.API
5) AcquiringBank.API

I have used MassTransit wrapper for RabbitMq as the messaging bus and Mongo Db as the query Db.

## Workflow

1) A merchant calls the WriteModel.API end point with a payment request and receives a payment id in response.

```json
{
    "CardNumber":"1234-1234-1234-1234",
    "Cvv": "123",
    "ExpiryDate":"01/2021" ,
    "OrderId" : "abcd",
    "Amount":12,
    "Currency":"£",
    "MerchantId": "merchant1" 
}
```

2) WriteModel.API does some validation and then sends a process payment command to WriteModel.Application

3) WriteModel.Application calls the acquiring bank API with the card details and then publishes the result.

4) This result is picked up by ReadModel.Denormalizer and saved in the database.

5) Merchant can then query ReadModel.API with the payment id to get back the status of the payment.


## Docker compose

In the checkout folder run  docker-compose up --build --renew-anon-volumes --abort-on-container-exit

This will start all the relevant services.

Rabbit MQ takes some time start and warm up.


## Swagger end points

For WriteModel.API-> http://localhost:5010/swagger/index.html

For ReadModel.API-> http://localhost:5012/swagger/index.html

### Debug notes

The first command to Rabbit goes in the skipped queue, this is just rabbit warming up. After the first one
things behave normally.

I have mocked the Acquiring bank API to return success with a new payment id always.

## Further enhancements needed

- Acceptance tests need somw work.
- Versionion of API
- Adding eventstore to store all events published from WriteModel.Application