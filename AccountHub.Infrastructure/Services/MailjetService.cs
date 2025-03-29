using AccountHub.Domain.Options;
using AccountHub.Domain.Services;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;

namespace AccountHub.Infrastructure.Services;

public class MailjetService:IMailjetService
{
    private readonly MailjetClient _client;

    public MailjetService(IOptions<MailjetOptions> mailjetOptions)
    {
        _client = new MailjetClient(mailjetOptions.Value.ApiKey,mailjetOptions.Value.SecretKey);
    }
    public async Task<string> SendEmailAsync(string to, string subject, long templateId, Dictionary<string, object> templateParams)
    {
        var email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact("ruslan74071@gmail.com"))
            .WithSubject(subject)
            .WithTemplateId(templateId)
            .WithVariables(templateParams)
            .WithTo(new SendContact(to))
            .Build();
        
        var response = await _client.SendTransactionalEmailAsync(email);
        Console.WriteLine("Response:"+response);
        return default;
    }
}