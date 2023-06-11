using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Commands
{
    public class CheckoutBookCommand :IRequest<ActionResult>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
