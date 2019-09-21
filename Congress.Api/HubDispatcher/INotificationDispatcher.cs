using Congress.Core.QueueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Congress.Api.HubDispatcher
{
    public interface INotificationDispatcher
    {
        Task SendEmailVerification(EmailVerificationQueueModel model);
        Task SendPassword(PasswordQueueModel model);
    }
}
