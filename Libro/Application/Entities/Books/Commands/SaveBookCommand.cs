using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Commands
{
    public class SaveBookCommand : IRequest<(Result, string)>
    {
        public int BookId { get; set; }
        public int ReadingListId { get; set; }
    }
}
