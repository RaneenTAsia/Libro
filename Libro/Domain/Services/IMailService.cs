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
        Task SendOverdueBookEmailsAsync(List<string> emails, List<string> books, List<decimal> Fines);
        Task SendCompletedReservationEmailAsync(string email, string book);
    }
}
