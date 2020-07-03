namespace PaymentGateway.WriteModel.API.Tests
{
    using FluentValidation.TestHelper;
    using Validators;
    using Xunit;

    public class PaymentValidatorTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("123412341234")]
        [InlineData("1234a242312")]
        [InlineData("1234-1234-1234-12341")]
        public void WhenCardNumberIsNullOrInWrongFormat_ShouldHaveError(string cardNumber)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.CardNumber, cardNumber);
        }
        
        [Theory]
        [InlineData("1234123412341234")]
        [InlineData("1234-1234-1234-1234")]
        public void WhenCardNumberIsValid_ShouldNotHaveError(string cardNumber)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.CardNumber, cardNumber);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("1234")]
        [InlineData("aaa")]
        public void WhenCvvIsNullOrInWrongFormat_ShouldHaveError(string cvv)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Cvv, cvv);
        }
        
        [Theory]
        [InlineData("123")]
        public void WhenCvvIsValid_ShouldHaveError(string cvv)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.Cvv, cvv);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("01/2009")]
        [InlineData("01/2019")]
        [InlineData("01/19")]
        public void WhenExpiryDateIsNullOrInWrongFormat_ShouldHaveError(string expiryDate)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.ExpiryDate, expiryDate);
        }
        
        [Theory]
        [InlineData("01/2021")]
        public void WhenExpiryDateIsValid_ShouldHaveError(string expiryDate)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.ExpiryDate, expiryDate);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void WhenOrderIdIsNull_ShouldHaveError(string orderId)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.OrderId, orderId);
        }
        
        [Theory]
        [InlineData("order-123 ")]
        public void WhenOrderIdIsValid_ShouldHaveError(string orderId)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.OrderId, orderId);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void WhenCurrencyIsNull_ShouldHaveError(string currency)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Currency, currency);
        }
        
        [Theory]
        [InlineData("£")]
        public void WhenCurrencyIsValid_ShouldHaveError(string currency)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.Currency, currency);
        }
        
        [Theory]
        [InlineData(-1)]
        public void WhenAmountIsNegative_ShouldHaveError(decimal amount)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Amount, amount);
        }
        
        [Theory]
        [InlineData(123)]
        public void WhenAmountIsValid_ShouldHaveError(decimal amount)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.Amount, amount);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void WhenMerchantIdIsInvalid_ShouldHaveError(string merchantId)
        {
            var sut = new PaymentRequestValidator();
            sut.ShouldHaveValidationErrorFor(x => x.MerchantId, merchantId);
        }
    }
}