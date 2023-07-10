using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendCancelledReservationsEmailsAsync(List<string> emails, List<string> books);
        Task SendCancelledReservationEmailAsync(string email, string book);
        Task SendOverdueBookEmailAsync(string email, string book, decimal fine);
        Task SendCompletedReservationEmailAsync(string email, string book);
    }
}
