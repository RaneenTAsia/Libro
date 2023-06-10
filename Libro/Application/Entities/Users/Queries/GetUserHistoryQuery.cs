using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Queries
{
    public class GetUserHistoryQuery : IRequest<(List<UserBorrowingHistoryFunctionResult>, string)>
    {
        public int UserId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
