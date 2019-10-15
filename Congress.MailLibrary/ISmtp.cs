using System.Threading.Tasks;

namespace Congress.MailLibrary
{
    public interface ISmtp
    {
        Task<bool> SendAsync(MailEntity mailEntity);
        bool Send(MailEntity mailEntity);
    }
}
