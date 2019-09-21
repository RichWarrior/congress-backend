using Congress.Api.Hubs;
using Congress.Core.QueueModel;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Congress.Api.HubDispatcher
{
    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationDispatcher(IHubContext<NotificationHub> _hubContext)
        {
            this._hubContext = _hubContext;
        }
        public async Task SendEmailVerification(EmailVerificationQueueModel model)
        {
            await this._hubContext.Clients.All.SendAsync("SendEmailVerification", model);
        }

        public async Task SendPassword(PasswordQueueModel model)
        {
            await this._hubContext.Clients.All.SendAsync("SendPassword", model);
        }
    }
}
