namespace AccountHub.Domain.Services;

public interface IMailjetService
{
    Task<string> SendEmailAsync(string to,string subject,long templateId,Dictionary<string,object> templateParams);
}