using Domain.Entities;

namespace Domain.Repositories
{
    public interface IViewOverdueBooksDetailsRepository
    {
        Task<List<ViewOverdueBookDetails>> GetOverdueBooksAsync();
    }
}