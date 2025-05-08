using EmailServiceProvider.Dtos;

namespace EmailServiceProvider.Factories
{
    public class EmailResultReplyFactory
    {
        public EmailReply CreateSucceesReply(EmailResultDto result)
        {
            return new EmailReply
            {
                Succeeded = result.IsSuccess,
                Result = result.Result
            };
        }

        public EmailReply CreateFailedReply(EmailResultDto result)
        {
            return new EmailReply
            {
                Succeeded = result.IsSuccess,
                Result = result.Result
            };
        }
    }
}
