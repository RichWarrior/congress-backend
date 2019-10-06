using Congress.Core.QueueModel;
using System.Threading.Tasks;

namespace Congress.Api.HubDispatcher
{
    public interface INotificationDispatcher
    {
        Task SendEmailVerification(EmailVerificationQueueModel model);
        Task SendPassword(PasswordQueueModel model);

        Task SendEventPushNotification(EventQueueModel model);
    }
}
