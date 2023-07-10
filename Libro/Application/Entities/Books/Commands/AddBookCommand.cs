using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Commands
{
    public class AddBookCommand : IRequest<ActionResult>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int PageAmount { get; set; }
        public DateTime PublishDate { get; set; }
        public List<int> Genres { get; set; }
        public List<int> BookAuthors { get; set; }
    }
}
