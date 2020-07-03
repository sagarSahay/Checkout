namespace PaymentGateway.WriteModel.API.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Commands;
    using Controllers;
    using FluentAssertions;
    using MassTransit;
    using Messages.Common;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using Profiles;
    using Xunit;

    public class PaymentGatewayControllerTest
    {
        private IMapper mapper;

        public PaymentGatewayControllerTest()
        {
            var myProfile = new ProcessPaymentProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task MakePayment_ReturnsBadResult_WhenModelStateIsInvalid()
        {
            // Arrange 
            var sendEndPoint = new Mock<ISendEndpoint>();
            var sut = new PaymentGatewayController(sendEndPoint.Object, mapper);
            sut.ModelState.AddModelError("CardNumber", "Required");
            var input = new PaymentRequest()
            {
                Cvv = "123",
                Amount = 1,
                Currency = "£",
                ExpiryDate = "01/2022",
                OrderId = "1234"
            };

            // Act
            var result = await sut.MakePayment(input);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            sendEndPoint.Verify(foo => foo.Send(It.IsAny<ICommand>(), new CancellationToken()), Times.Never);
        }

        [Fact]
        public async Task MakePayment_DoesNotSendCommand_WhenModelStateIsInvalid()
        {
            // Arrange 
            var sendEndPoint = new Mock<ISendEndpoint>();
            var sut = new PaymentGatewayController(sendEndPoint.Object, mapper);
            sut.ModelState.AddModelError("CardNumber", "Required");
            var input = new PaymentRequest()
            {
                Cvv = "123",
                Amount = 1,
                Currency = "£",
                ExpiryDate = "01/2022",
                OrderId = "1234"
            };

            // Act
            await sut.MakePayment(input);

            // Assert
            sendEndPoint.Verify(foo => foo.Send(It.IsAny<ICommand>(), new CancellationToken()), Times.Never);
        }

        [Fact]
        public async Task MakePayment_ReturnsAcceptedResult_WhenModelStateIsValid()
        {
            // Arrange 
            var sendEndPoint = new Mock<ISendEndpoint>();
            var sut = new PaymentGatewayController(sendEndPoint.Object, mapper);
            var input = new PaymentRequest()
            {
                CardNumber = "1234-1234-1234-1234",
                Cvv = "123",
                Amount = 1,
                Currency = "£",
                ExpiryDate = "01/2022",
                OrderId = "1234"
            };

            // Act
            var result = await sut.MakePayment(input);

            // Assert
            result.Should().BeOfType<AcceptedResult>();
        }

        [Fact]
        public async Task MakePayment_SendsCommand_WhenModelStateIsValid()
        {
            // Arrange 
            var commandList = new List<ProcessPayment>();
            var sendEndPoint = new Mock<ISendEndpoint>();
            sendEndPoint.Setup(x => x.Send(It.IsAny<ProcessPayment>(), new CancellationToken()))
                .Callback<ProcessPayment, CancellationToken>((c,ct) => commandList.Add(c));
            var sut = new PaymentGatewayController(sendEndPoint.Object, mapper);
            var input = new PaymentRequest()
            {
                CardNumber = "1234-1234-1234-1234",
                Cvv = "123",
                Amount = 1,
                Currency = "£",
                ExpiryDate = "01/2022",
                OrderId = "1234",
                MerchantId = "merchant1"
            };

            // Act
            await sut.MakePayment(input);

            // Assert
            sendEndPoint.Verify(foo => foo.Send(It.IsAny<ICommand>(), new CancellationToken()), Times.Once);
            var cmd = commandList.FirstOrDefault();
            cmd.Amount.Should().Be(input.Amount);
            cmd.Currency.Should().Be(input.Currency);
            cmd.Cvv.Should().Be(input.Cvv);
            cmd.CardNumber.Should().Be(input.CardNumber);
            cmd.OrderId.Should().Be(input.OrderId);
            cmd.MerchantId.Should().Be(input.MerchantId);
        }
    }
}