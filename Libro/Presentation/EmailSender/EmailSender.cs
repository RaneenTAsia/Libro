using Domain.Repositories;
using Domain.Services;

namespace Presentation
{
    public class EmailSender 
    {
        private readonly IServiceProvider service;
        private readonly IBookRepository bookRepo;
        private readonly IBookReservationRepository bookReservationRepo;
        private readonly IUserRepository userRepo;
        private readonly IMailService mailService;
        private readonly IBookTransactionRepository bookTransactionRepo;

        public EmailSender(IServiceProvider service)
        {
            this.service = service;
            bookReservationRepo = service.GetService<IBookReservationRepository>();
            userRepo = service.GetService<IUserRepository>();
            bookRepo = service.GetService<IBookRepository>();
            mailService = service.GetService<IMailService>();
            bookTransactionRepo = service.GetService<IBookTransactionRepository>();
        }

        public async Task SendCancelledReservationEmailsAsync()
        {

            //cancelled reservations emails
            var removedReservations = await bookReservationRepo.RemoveBookReservationsAsync();
            var emails = await userRepo.GetUserEmailsByIdsAsync(removedReservations.Select(r => r.UserId).ToList());
            var bookTitles = (await bookRepo.GetBooksByIdsAsync(removedReservations.Select(r => r.BookId).ToList())).Select(b => b.Title).ToList();
            await mailService.SendCancelledReservationsEmailsAsync(emails, bookTitles);
        }

        public async Task SendOverdueBookEmailsAsync()
        {
            var overdueBooks = await bookTransactionRepo.GetOverdueBookTransactionsAsync();
            var emails = await userRepo.GetUserEmailsByIdsAsync(overdueBooks.Select(r => r.UserId).ToList());
            var bookTitles = (await bookRepo.GetBooksByIdsAsync(overdueBooks.Select(r => r.BookId).ToList())).Select(b => b.Title).ToList();
            var fines = overdueBooks.Select(o => o.Fine).ToList();
            await mailService.SendOverdueBookEmailsAsync(emails, bookTitles, fines);
        }
    }
}
