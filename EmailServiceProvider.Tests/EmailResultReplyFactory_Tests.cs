using EmailServiceProvider.Dtos;
using EmailServiceProvider.Factories;

namespace EmailServiceProvider.Tests
{
    public class EmailResultReplyFactory_Tests
    {
        [Fact]
        public void CreateSuccessReply_ShouldReturnSuccessReply()
        {
            // Arrange
            var factory = new EmailResultReplyFactory();
            var result = new EmailResultDto { IsSuccess = true, Result = "Email sent successfully." };

            // Act
            var reply = factory.CreateSucceesReply(result);

            // Assert
            Assert.True(reply.Succeeded);
            Assert.Equal("Email sent successfully.", reply.Result);
        }

        [Fact]
        public void CreateFailedReply_ShouldReturnFailedReply()
        {
            // Arrange
            var factory = new EmailResultReplyFactory();
            var result = new EmailResultDto { IsSuccess = false, Result = "Email sending failed." };

            // Act
            var reply = factory.CreateFailedReply(result);

            // Assert
            Assert.False(reply.Succeeded);
            Assert.Equal("Email sending failed.", reply.Result);
        }
    }
}
