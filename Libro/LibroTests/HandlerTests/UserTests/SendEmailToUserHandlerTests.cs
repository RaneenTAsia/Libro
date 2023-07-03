using Application.Entities.Users.Handlers;
using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.HandlerTests.UserTests
{
    public class SendEmailToUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly Mock<ILogger<SendEmailToUserHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SendEmailToUserHandler _handler;

        public SendEmailToUserHandlerTests()
        {
            _loggerMock = new Mock<ILogger<SendEmailToUserHandler>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mailServiceMock = new Mock<IMailService>();
            _mapperMock = new Mock<IMapper>();

            _handler = new SendEmailToUserHandler(
                _userRepositoryMock.Object,
                _mailServiceMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var query = new SendEmailToUserQuery
            {
                Subject = "Test",
                ToEmail = "Test",
                Body = "Test"
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(query.ToEmail))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("This email does not exist for any user", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_SendEmail_ReturnsOkObjectResult()
        {
            // Arrange
            var query = new SendEmailToUserQuery
            {
                Subject = "Test",
                ToEmail = "Test",
                Body = "Test"
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(query.ToEmail))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(m => m.Map<MailRequest>(query))
                .Returns(new MailRequest());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Email successfully sent", (result as OkObjectResult)?.Value);
        }
    }
}
