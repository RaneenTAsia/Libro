using Application.Entities.Users.Handlers;
using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<SendEmailToUserQuery, MailRequest>();
        }
    }
}
