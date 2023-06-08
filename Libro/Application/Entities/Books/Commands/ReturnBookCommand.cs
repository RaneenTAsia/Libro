using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Commands
{
    public class ReturnBookCommand : IRequest<(TransactionToReturnForBookReturnDTO, string)>
    {
        public int BookId { get; set; }
    }
}
