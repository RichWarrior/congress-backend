using Congress.Core.Entity;
using Congress.Core.Interface;
using Congress.Core.ParameterModel;
using Congress.Core.QueueModel;
using Congress.Data.Data;
using Congress.Data.Service;
using Congress.MailLibrary;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
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
        IUser _SUser;
        ISmtp _SSmtp;

        Connection connection = new Connection();
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
            _SUser = new SUser(new DbContext());


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
                do
                {
                    EmailVerificationQueueModel emailVerificationQueueModel;
                    emailVerificationQueues.TryDequeue(out emailVerificationQueueModel);
                    if (emailVerificationQueueModel!=null)
                    {
                        string path = String.Format("{0}/ActivationAccount/{1}",connection.weburl,emailVerificationQueueModel.userGuid);
                        MailEntity mailEntity = new MailEntity()
                        {
                            subject = "Email Aktivasyon İşlemi",
                            to = emailVerificationQueueModel.email,
                            body  =String.Format("{0} E-Postalı Hesabınızı Oluşturduk. Giriş Yapabilmeniz İçin Sadece Aşağıdaki Bağlantıdan E-Posta Adresinizi Onaylamanız Gerekmektedir!\n" +
                            "<a href='{1}'>Hesabı Aktifleştirmek İçin Buraya Tıkla</a>",emailVerificationQueueModel.email,path)
                        };
                        Task.Run(() => _SSmtp.SendAsync(mailEntity));
                        string json = JsonConvert.SerializeObject(emailVerificationQueueModel);
                        Console.WriteLine(String.Format("Kullanıcı E-Posta Aktivasyon Maili Gönderildi!\n{0}",json));
                    }
                } while (emailVerificationQueues.Count>0);
            }
            Monitor.Exit(state);
        }

        private void PasswordTick(object state)
        {
            if (!Monitor.TryEnter(state))
                return;
            if (_SSmtp != null)
            {
                do
                {
                    PasswordQueueModel passwordQueueModel;
                    passwordQueues.TryDequeue(out passwordQueueModel);
                    if (passwordQueueModel!=null)
                    {
                        MailEntity mailEntity = new MailEntity()
                        {
                            subject = "Şifre Yenileme İsteği",
                            body = String.Format("İsteğiniz Doğrultusunda {0} Hesabınızın Şifresi {1} Olarak Güncellenmiştir.",passwordQueueModel.email,passwordQueueModel.password),
                            to = passwordQueueModel.email
                        };
                        Task.Run(() => _SSmtp.SendAsync(mailEntity));
                        string _json = JsonConvert.SerializeObject(mailEntity);
                        Console.WriteLine(String.Format("Kullanıcının Yeni Şifresi Gönderildi!\n{0}",_json));
                    }
                    
                } while (passwordQueues.Count>0);
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
                            string _json = JsonConvert.SerializeObject(mailEntity);
                            Console.WriteLine(String.Format("Event Bildirim Gönderiliyor!\n{0}",_json));
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
                do
                {
                    EventParticipantRequestQueueModel eventParticipantRequestQueueModel;
                    eventParticipantRequestQueues.TryDequeue(out eventParticipantRequestQueueModel);
                    if (eventParticipantRequestQueueModel!=null)
                    {
                        Event _event = _SEvent.GetById(eventParticipantRequestQueueModel.eventId);
                        User creatorUser = _SUser.GetById(_event.userId);
                        User _user = _SUser.GetById(eventParticipantRequestQueueModel.userId);
                        MailEntity mailEntity = new MailEntity()
                        {
                            subject = String.Format("{0} Etkinliğine Katılımcı İsteği", _event.name),
                            body = String.Format("{0} {1} Kullanıcısı {2} İsimli Düzenlemiş Olduğunuz Etkinliğe Katılmak İstiyor.",_user.name,_user.surname,_event.name),
                            to = creatorUser.email
                        };
                        Task.Run(() => _SSmtp.SendAsync(mailEntity));
                        string _json = JsonConvert.SerializeObject(mailEntity);
                        Console.WriteLine(String.Format("Etkinlik Katılımcı İsteği Gönderildi!\n{0}",_json));
                    }
                } while (eventParticipantRequestQueues.Count>0);
            }
            Monitor.Exit(state);
        }
    }
}
