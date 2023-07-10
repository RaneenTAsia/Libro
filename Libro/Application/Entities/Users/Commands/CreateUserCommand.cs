using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Commands
{
    public class CreateUserCommand : IRequest<ActionResult>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
