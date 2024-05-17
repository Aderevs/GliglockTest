﻿using AutoMapper;
using GliglockTest.appCore.Account;
using System.Linq.Expressions;

namespace GliglockTest.appCore
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Test, DbLogic.Test>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<PassedTest, DbLogic.PassedTest>()
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.Test.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
                .ForMember(dest => dest.Test, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());

            CreateMap<Question, DbLogic.Question>()
                .ForMember(dest => dest.AnswerOptions, opt => opt.Ignore());

            CreateMap<AnswerOption, DbLogic.AnswerOption>();


            CreateMap<DbLogic.Test, Test>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<DbLogic.PassedTest, PassedTest>()
                .ForMember(dest => dest.Student, opt => opt.Ignore());

            CreateMap<DbLogic.Question, Question>();
            CreateMap<DbLogic.AnswerOption, AnswerOption>();

            CreateMap<DbLogic.Student, Models.StudentView>();
            CreateMap<DbLogic.Teacher, Models.TeacherView>();
        }
    }
}
