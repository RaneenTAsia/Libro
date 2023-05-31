using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class ChangeUserRoleCommand : IRequest<(Result,string)>
    {
        public int UserId { get; set; }
        public int Role { get; set; }
    }
}
