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
        Queue<PasswordQueueModel> passwordQueues;
        Timer passwordTimer;
        object passwordLockObject;
        Queue<EventQueueModel> eventQueues;
        Timer eventTimer;
        object eventLockObject;
        Queue<EventParticipantRequestQueueModel> eventParticipantRequestQueues;
        Timer eventParticipantTimer;
        object eventParticipantLockObject;
        #endregion

        public NotificationService()
        {
            _connection = new Connection();
            emailVerificationQueues = new Queue<EmailVerificationQueueModel>();
            emailVerificationLockObject = new object();

            passwordQueues = new Queue<PasswordQueueModel>();
            passwordLockObject = new object();

            eventQueues = new Queue<EventQueueModel>();
            eventLockObject = new object();

            eventParticipantRequestQueues = new Queue<EventParticipantRequestQueueModel>();
            eventParticipantLockObject = new object();

            HubConnection connection = new HubConnectionBuilder().WithUrl(_connection.apiUrl + "/NotificationHub").Build();
            connection.StartAsync().ContinueWith(task =>
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

            connection.On<EventQueueModel>("sendEventPushNotification", (item) =>
            {
                eventQueues.Enqueue(item);
            });

            connection.On<EmailVerificationQueueModel>("SendEmailVerification", (item) =>
            {
                emailVerificationQueues.Enqueue(item);
            });

            connection.On<PasswordQueueModel>("SendPassword", (item) =>
            {
                passwordQueues.Enqueue(item);
            });

            connection.On<EventParticipantRequestQueueModel>("sendEventParticipantRequest", (item) =>
            {
                eventParticipantRequestQueues.Enqueue(item);
            });

            emailVerificaitonTimer = new Timer(EmailVerificationTick, emailVerificationLockObject, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            passwordTimer = new Timer(PasswordTick, passwordLockObject, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            eventTimer = new Timer(EventTick, eventLockObject, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            eventParticipantTimer = new Timer(EventParticipantTick,eventParticipantLockObject,TimeSpan.Zero,TimeSpan.FromSeconds(60));
        }

        private void EmailVerificationTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            Monitor.Exit(state);
        }

        private void PasswordTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            Monitor.Exit(state);
        }

        private void EventTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;            
            Monitor.Exit(state);
        }

        private void EventParticipantTick(object state)
        {

        }
    }
}
