using Application.Configurations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Queries
{
    public class GetBookRecommendationsQuery : IRequest<(List<ViewBooks>, PaginationMetadata)>
    {
        public int UserId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
