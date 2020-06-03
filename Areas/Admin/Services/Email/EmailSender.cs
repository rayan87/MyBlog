using System.Threading.Tasks;

namespace MyBlog.Admin.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendAsync(string email, string subject, string message)
        {
            throw new System.NotImplementedException();
        }
    }
}