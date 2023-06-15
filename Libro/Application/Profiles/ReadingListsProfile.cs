using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class ReadingListsProfile : Profile
    {
        public ReadingListsProfile()
        {
            CreateMap<ReadingList, ReadingListDTO>();
            CreateMap<AddReadingListCommand, ReadingList>();
        }
    }
}
