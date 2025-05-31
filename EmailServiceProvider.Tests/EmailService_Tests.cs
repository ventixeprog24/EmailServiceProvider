using Azure;
using Azure.Communication.Email;
using EmailServiceProvider;
using EmailServiceProvider.Dtos;
using EmailServiceProvider.Factories;
using EmailServiceProvider.Services;
using Microsoft.Extensions.Logging;
using Moq;

public class EmailServiceTests
{
    // Mycket AI genererad kod
    [Fact]
    public async Task SendEmail_ShouldReturnSuccessReply_WhenEmailIsSentSuccessfully()
    {
        // Arrange
        var emailClientMock = new Mock<EmailClient>();
        var loggerMock = new Mock<ILogger<EmailService>>();
        var replyFactoryMock = new Mock<EmailResultReplyFactory>();

        var expectedReply = new EmailReply();

        var emailSendOperationMock = new Mock<EmailSendOperation>();
        emailSendOperationMock.Setup(x => x.HasCompleted).Returns(true);

        emailClientMock
            .Setup(client => client.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emailSendOperationMock.Object);

        replyFactoryMock
            .Setup(f => f.CreateSucceesReply(It.Is<EmailResultDto>(dto => dto.IsSuccess)))
            .Returns(expectedReply);

        var service = new EmailService(emailClientMock.Object, loggerMock.Object, replyFactoryMock.Object);

        var request = new EmailRequest
        {
            Recipients = { "test@example.com" },
            SenderAddress = "sender@example.com",
            Subject = "Test Subject",
            PlainText = "Plain text",
            Html = "<p>HTML</p>"
        };

        // Act
        var result = await service.SendEmail(request, null);

        // Assert
        Assert.Equal(expectedReply, result);
    }

    [Fact]
    public async Task SendEmail_ShouldReturnFailedReply_WhenEmailIsNotCompleted()
    {
        // Arrange
        var emailClientMock = new Mock<EmailClient>();
        var loggerMock = new Mock<ILogger<EmailService>>();
        var replyFactoryMock = new Mock<EmailResultReplyFactory>();

        var expectedReply = new EmailReply();

        var emailSendOperationMock = new Mock<EmailSendOperation>();
        emailSendOperationMock.Setup(x => x.HasCompleted).Returns(false);

        emailClientMock
            .Setup(client => client.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emailSendOperationMock.Object);

        replyFactoryMock
            .Setup(f => f.CreateFailedReply(It.Is<EmailResultDto>(dto => !dto.IsSuccess)))
            .Returns(expectedReply);

        var service = new EmailService(emailClientMock.Object, loggerMock.Object, replyFactoryMock.Object);

        var request = new EmailRequest
        {
            Recipients = { "test@example.com" },
            SenderAddress = "sender@example.com",
            Subject = "Test Subject",
            PlainText = "Plain text",
            Html = "<p>HTML</p>"
        };

        // Act
        var result = await service.SendEmail(request, null);

        // Assert
        Assert.Equal(expectedReply, result);
    }

    [Fact]
    public async Task SendEmail_ShouldReturnFailedReply_WhenRequestFailedExceptionIsThrown()
    {
        // Arrange
        var emailClientMock = new Mock<EmailClient>();
        var loggerMock = new Mock<ILogger<EmailService>>();
        var replyFactoryMock = new Mock<EmailResultReplyFactory>();

        var expectedReply = new EmailReply();
        var exception = new RequestFailedException(500, "Azure down");

        emailClientMock
            .Setup(client => client.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        replyFactoryMock
            .Setup(f => f.CreateFailedReply(It.Is<EmailResultDto>(dto => !dto.IsSuccess && dto.Result == "Azure down")))
            .Returns(expectedReply);

        var service = new EmailService(emailClientMock.Object, loggerMock.Object, replyFactoryMock.Object);

        var request = new EmailRequest
        {
            Recipients = { "test@example.com" },
            SenderAddress = "sender@example.com",
            Subject = "Test Subject",
            PlainText = "Plain text",
            Html = "<p>HTML</p>"
        };

        // Act
        var result = await service.SendEmail(request, null);

        // Assert
        Assert.Equal(expectedReply, result);
    }

    [Fact]
    public async Task SendEmail_ShouldReturnFailedReply_WhenUnexpectedExceptionIsThrown()
    {
        // Arrange
        var emailClientMock = new Mock<EmailClient>();
        var loggerMock = new Mock<ILogger<EmailService>>();
        var replyFactoryMock = new Mock<EmailResultReplyFactory>();

        var expectedReply = new EmailReply();
        var exception = new Exception("Unexpected error");

        emailClientMock
            .Setup(client => client.SendAsync(It.IsAny<WaitUntil>(), It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        replyFactoryMock
            .Setup(f => f.CreateFailedReply(It.Is<EmailResultDto>(dto => !dto.IsSuccess && dto.Result == "Unexpected error")))
            .Returns(expectedReply);

        var service = new EmailService(emailClientMock.Object, loggerMock.Object, replyFactoryMock.Object);

        var request = new EmailRequest
        {
            Recipients = { "test@example.com" },
            SenderAddress = "sender@example.com",
            Subject = "Test Subject",
            PlainText = "Plain text",
            Html = "<p>HTML</p>"
        };

        // Act
        var result = await service.SendEmail(request, null);

        // Assert
        Assert.Equal(expectedReply, result);
    }


}
