using Congress.Core.Entity;
using Congress.Core.Interface;
using Congress.Core.ParameterModel;
using Congress.Core.QueueModel;
using Congress.Data.Data;
using Congress.Data.Service;
using Congress.MailLibrary;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        ISystemParameter _SSystemParameter;
        IMethod _SMethod;
        IEvent _SEvent;
        IEventParticipant _SEventParticipant;
        ISmtp _SSmtp;
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

            _SSystemParameter = new SSystemParameter(new DbContext());
            _SMethod = new SMethod();
            _SEvent = new SEvent(new DbContext());
            _SEventParticipant = new SEventParticipant(new DbContext());


            List<SystemParameter> systemParameters = _SSystemParameter.GetSystemParameter();
            SmtpModel smtpModel = _SMethod.SystemParameterToObject<SmtpModel>(systemParameters);
            if (!String.IsNullOrEmpty(smtpModel.smtp_host))
            {
                _SSmtp = new SSmtp(smtpModel.smtp_host, smtpModel.smtp_port, smtpModel.smtp_sender, smtpModel.smtp_password);
            }
        }

        private void EmailVerificationTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            if (_SSmtp!=null)
            {

            }
            Monitor.Exit(state);
        }

        private void PasswordTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            if (_SSmtp != null)
            {

            }
            Monitor.Exit(state);
        }

        private void EventTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            if (_SSmtp != null)
            {
                do
                {
                    EventQueueModel eventQueueModel;
                    eventQueues.TryDequeue(out eventQueueModel);
                    if (eventQueueModel!=null)
                    {
                        Event _event = _SEvent.GetById(eventQueueModel.eventId);
                        List<User> eventParticipants = _SEventParticipant.GetEventParticipants(_event.id);
                        foreach (var item in eventParticipants)
                        {
                            MailEntity mailEntity = new MailEntity()
                            {
                                subject = String.Format("{0} Etkinliği Hakkında Bilgilendirme", _event.name),
                                body = eventQueueModel.message,
                                to = item.email
                            };
                            Task.Run(() => _SSmtp.SendAsync(mailEntity));
                        }
                    }
                } while (eventQueues.Count>0);
            }
            Monitor.Exit(state);
        }

        private void EventParticipantTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            if (_SSmtp!=null)
            {

            }
            Monitor.Exit(state);
        }
    }
}
