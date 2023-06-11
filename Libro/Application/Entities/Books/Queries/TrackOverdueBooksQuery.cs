using Application.Configurations;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Books.Queries
{
    public class TrackOverdueBooksQuery : IRequest<(List<ViewOverdueBookDetails>, PaginationMetadata)>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
