using Application.Configurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Reviews.Queries
{
    public class GetReviewsQuery : IRequest<(ActionResult, PaginationMetadata)>
    {
        public int BookId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
