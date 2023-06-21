using AutoDependencyRegistration.Attributes;
using Domain.Models;
using Domain.Services;
using Infrastructure.Persistence.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


[RegisterClassAsScoped]
public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = mailRequest.Body;
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }

    public async Task SendCancelledReservationsEmailsAsync(List<string> emails, List<string> books)
    {
        var request = new MailRequest();
        request.Subject = "Reservation Cancelled";
        
        for(int i = 0; i< emails.Count; i++)
        {
            request.Body = $"Hello, we would just like to inform you of the cancellation of your Book Reservation of '{books[i]}'.";
            request.ToEmail = emails[i];
            await SendEmailAsync(request);
        }
    }

    public async Task SendOverdueBookEmailsAsync(List<string> emails, List<string> books, List<decimal> fines)
    {
        var request = new MailRequest();
        request.Subject = "Overdue Book";

        for (int i = 0; i < emails.Count; i++)
        {
            request.Body = $"Hello, we would just like to inform you tha you are in possession of the overdue Book '{books[i]}'. \n" +
                $"The book currently has a fine of {fines[i]}. \n We kindly request for it to be returned as soon as you are able to. \n" +
                $"Every overdue day results in a 0.05 cent increase to you fine.\n" +
                $"Have a great day";
            request.ToEmail = emails[i];
            await SendEmailAsync(request);
        }
    }

    public async Task SendCompletedReservationEmailAsync(string email, string book)
    {
        var request = new MailRequest();
        request.Subject = "Reservation Completed";

        request.Body = $"Hello, we would just like to inform you of your Book Reservation for the book '{book}'";
        request.ToEmail = email;

        await SendEmailAsync(request);
    }
}