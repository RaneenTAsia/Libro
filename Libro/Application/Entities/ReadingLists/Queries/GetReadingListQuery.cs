using Application.Configurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.ReadingLists.Queries
{
    public class GetReadingListQuery : IRequest<(ActionResult, PaginationMetadata)>
    {
        public int UserId { get; set; }
        public int ReadingListId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
