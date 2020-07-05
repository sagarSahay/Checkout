Feature: GetPaymentDetails
  In order to give merchant details about payment
  As a payment gateway
  We should be able to process payment and provide them with an id to query the status of payment


  Background:
    Given 'merchant1' is registered with LLoyds bank

  @acceptance
  Scenario: Process A payment
    Given the following details about an order
      | OrderId | MerchantId | CardNumber          | Cvv | ExpiryDate | Amount | Currency |
      | order1  | merchant1  | 1234-1234-1234-1234 | 123 | 02/2021    | 12     | $        |
    When payment gateway processes a payment
    Then an accepted result is returned with a payment id