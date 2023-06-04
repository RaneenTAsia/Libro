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
    public class BrowseAvailableBooksQuery : IRequest<(List<BrowsingBookDTO>, PaginationMetadata)>
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
