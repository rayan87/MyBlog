using System.Threading.Tasks;

namespace MyBlog.Admin.Services.Email
{
    public interface IEmailSender
    {
        Task SendAsync(string email, string subject, string message);
    } 
}