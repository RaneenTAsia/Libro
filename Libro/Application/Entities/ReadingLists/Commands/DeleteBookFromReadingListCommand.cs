using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.ReadingLists.Commands
{
    public class DeleteBookFromReadingListCommand : IRequest<ActionResult>
    {
        public int BookId { get; set; }
        public int ReadingListId { get; set; }
    }
}
