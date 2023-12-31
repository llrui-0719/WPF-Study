﻿using AutoMapper.Configuration;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api.Extensions
{
    public class AutoMapperProfile:MapperConfigurationExpression
    {
        public AutoMapperProfile()
        {
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
