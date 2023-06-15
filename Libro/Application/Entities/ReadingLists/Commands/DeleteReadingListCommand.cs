using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.ReadingLists.Commands
{
    public class DeleteReadingListCommand : IRequest<ActionResult>
    {
        public int ReadingListId { get; set; }
    }
}
