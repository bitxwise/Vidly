﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Data;
using Vidly.Models;

namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Customer, CustomerData>();
            Mapper.CreateMap<CustomerData, Customer>();
            Mapper.CreateMap<Movie, MovieData>();
            Mapper.CreateMap<MovieData, Movie>();
        }
    }
}