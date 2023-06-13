using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Authors.Commands
{
    public class AddAuthorCommand : IRequest<(Result, string)>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int>? BookIds { get; set; }
    }
}
