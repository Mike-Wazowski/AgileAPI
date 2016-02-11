using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSST.Agile.Database.Models;
using TSST.Agile.Models;
using TSST.Agile.Web.Helpers;

namespace TSST.Agile.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void InitializeAutoMapper()
        {
            Mapper.Reset();
            Mapper.CreateMap<User, ApplicationUser>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.PictureUrl));
            Mapper.AssertConfigurationIsValid();
        }
    }
}