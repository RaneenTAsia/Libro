﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Reviews.Commands
{
    public class DeleteReviewCommand : IRequest<ActionResult>
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
