﻿using AutoMapper;
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
            Mapper.CreateMap<User, UserViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.PictureUrl));
            Mapper.CreateMap<Project, ProjectViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
            Mapper.CreateMap<ProjectViewModel, Project>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
            Mapper.CreateMap<File, FileViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl));
            Mapper.CreateMap<Task, TaskViewModel>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.CompleteDate, opt => opt.MapFrom(src => src.CompleteDate))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => Mapper.Map<IEnumerable<File>, ICollection<FileViewModel>>(src.Files)));
            Mapper.CreateMap<TaskViewModel, Task>()
                .IgnoreAllUnmapped()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            Mapper.AssertConfigurationIsValid();
        }
    }
}