using Domain.Entities;

namespace Domain.Repositories
{
    public interface IViewBooksRepository
    {
        Task<List<ViewBooks>> GetBooksAsync();
        List<ViewBooks> GetBooksWithAuthor(string author);
        List<ViewBooks> GetBooksWithTitle(string title);
    }
}