using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Authors.Commands
{
    public class UpdateAuthorCommand : IRequest<ActionResult>
    {
        public int AuthorId { get; set; }
        public AuthorRetrievalDTO RetrievedAuthorDTO { get; set; }
    }
}
