using Grpc.Core;
using Azure.Communication.Email;
using Azure;
using EmailServiceProvider.Dtos;
using EmailServiceProvider.Factories;

namespace EmailServiceProvider.Services;

public class EmailService(EmailClient emailClient, ILogger<EmailService> logger, EmailResultReplyFactory emailResultReplyFactory) : EmailServicer.EmailServicerBase
{
    private readonly EmailClient _emailClient = emailClient;
    private readonly ILogger<EmailService> _logger = logger;
    private readonly EmailResultReplyFactory _emailResultReplyFactory = emailResultReplyFactory;

    public override async Task<EmailReply> SendEmail(EmailRequest request, ServerCallContext context)
    {
        try
        {
            var recipients = request.Recipients.Select(email => new EmailAddress(email)).ToList();

            var emailMessage = new EmailMessage(
                recipients: new EmailRecipients(recipients),
                senderAddress: request.SenderAddress,
                content: new EmailContent(request.Subject)
                {
                    PlainText = request.PlainText,
                    Html = request.Html
                });

            var response = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);

            if (!response.HasCompleted)
            {
                _logger.LogError($"Sending email failed.");
                var result = new EmailResultDto { IsSuccess = false, Result = "Sending email failed." };
                return _emailResultReplyFactory.CreateFailedReply(result);
            }
            else
            {
                _logger.LogInformation($"Email sent successfully.");
                var result = new EmailResultDto {  IsSuccess = true, Result = "Email sent succesfully."};
                return _emailResultReplyFactory.CreateSucceesReply(result);
            }
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError($"Azure Request failed. Status: {ex.Status}. Message: {ex.Message}");
            var result = new EmailResultDto { IsSuccess = false, Result = ex.Message };
            return _emailResultReplyFactory.CreateFailedReply(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred. Message: {ex.Message}");
            var result = new EmailResultDto { IsSuccess = false, Result = ex.Message };
            return _emailResultReplyFactory.CreateFailedReply(result);
        }
    }
}
