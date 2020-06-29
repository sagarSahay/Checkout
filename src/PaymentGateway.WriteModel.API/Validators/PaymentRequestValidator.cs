namespace PaymentGateway.WriteModel.API.Validators
{
    using System;
    using System.Text.RegularExpressions;
    using FluentValidation;
    using Models;

    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        //TODO: Consider refactoring using DI for different validators
        public PaymentRequestValidator()
        {
            RuleFor(x => x.CardNumber).Must(BeValidCard)
                .WithMessage("Card number is not valid");
            RuleFor(x => x.Cvv).Must(BeValidCvv);
            RuleFor(x => x.ExpiryDate).Must(BeValidExpiryDate);
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Amount).Must(BeValidAmount);
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.MerchantId).NotEmpty();
        }

        private bool BeValidAmount(decimal amount)
        {
            return amount > 0;
        }

        private bool BeValidExpiryDate(string expiryDate)
        {
            if (string.IsNullOrEmpty(expiryDate))
            {
                return false;
            }
            var monthPattern = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearPattern = new Regex(@"^20[0-9]{2}$");

            var dateParts = expiryDate.Split('/');
            if (dateParts.Length != 2)
            {
                return false;
            }
            if (!monthPattern.IsMatch(dateParts[0]) || !yearPattern.IsMatch(dateParts[1]))
            {
                return false;
            }

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now);
        }

        private bool BeValidCvv(string cvv)
        {
            if (string.IsNullOrEmpty(cvv))
            {
                return false;
            }
            var cvvPattern = new Regex(@"^\d{3}$");
            return cvvPattern.IsMatch(cvv);
        }

        private bool BeValidCard(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                return false;
            }
            var cardNumberPattern = new Regex(@"([\-\s]?[0-9]{4}){4}$");
            return cardNumberPattern.IsMatch(cardNumber);
        }
    }
}