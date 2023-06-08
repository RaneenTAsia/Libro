using Application.Configurations;
using Application.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Queries
{
    public class SearchBooksQuery : IRequest<(List<BrowsingBookDTO>, PaginationMetadata)>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? GenreId { get; set; }
        public int pageNumber { get; set; } 
        public int pageSize { get; set; } 
    }
}
