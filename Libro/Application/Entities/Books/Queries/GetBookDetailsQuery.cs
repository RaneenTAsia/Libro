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
    public class GetBookDetailsQuery :IRequest<BookDetailsDTO>
    {
        public int BookId { get; set; }
    }
}
