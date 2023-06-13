using Application.DTOs;
using Application.Entities.Authors.Commands;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AddAuthorCommand, Author>();
            CreateMap<AuthorRetrievalDTO, AuthorUpdateDTO>();
            CreateMap<AuthorUpdateDTO, Author>();
            CreateMap<Author, AuthorDTO>();
        }
    }
}
