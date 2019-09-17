using Congress.Core.QueueModel;
using Congress.Data.Data;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Congress.PushNotification
{
    public class NotificationService
    {
        #region Variables
        Connection _connection;
        Queue<EmailVerificationQueueModel> emailVerificationQueues;
        Timer emailVerificaitonTimer;
        object emailVerificationLockObject;
        #endregion

        public NotificationService()
        {
            _connection = new Connection();
            emailVerificationQueues = new Queue<EmailVerificationQueueModel>();
            emailVerificationLockObject = new object();

            HubConnection hubConnection = new HubConnectionBuilder().WithUrl(_connection.apiUrl+"/NotificationHub").Build();
            hubConnection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {                    
                    Console.WriteLine(task.Exception);
                }
                else
                {
                    Console.WriteLine("SignalR Connected...");
                }
            });

            emailVerificaitonTimer = new Timer(EmailVerificationTick,emailVerificationLockObject,TimeSpan.Zero,TimeSpan.FromSeconds(10));
        }

        private void EmailVerificationTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
        }
    }
}
