using EmailServiceProvider.Dtos;

namespace EmailServiceProvider.Factories
{
    public class EmailResultReplyFactory
    {
        public virtual EmailReply CreateSucceesReply(EmailResultDto result)
        {
            return new EmailReply
            {
                Succeeded = result.IsSuccess,
                Result = result.Result
            };
        }

        public virtual EmailReply CreateFailedReply(EmailResultDto result)
        {
            return new EmailReply
            {
                Succeeded = result.IsSuccess,
                Result = result.Result
            };
        }
    }
}
