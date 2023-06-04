using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Commands
{
    public class ReserveBookCommand :IRequest<(Result,string)>
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
