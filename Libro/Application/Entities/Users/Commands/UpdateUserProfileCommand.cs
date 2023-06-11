using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Commands
{
    public class UpdateUserProfileCommand : IRequest<ActionResult>
    {
        public int UserId { get; set; }
        public ProfileToUpdateDTO Profile { get; set; }
        public int TokenUserId { get; set; }
        public string TokenUserRole { get; set; }
    }
}
