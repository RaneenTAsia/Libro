using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Queries
{
    public class AuthenticateUserQuery : IRequest<ActionResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
