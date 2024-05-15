using AutoMapper;
using GliglockTest.appCore.Account;
using System.Linq.Expressions;

namespace GliglockTest.appCore
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Test, DbLogic.Test>();
            CreateMap<PassedTest, DbLogic.PassedTest>()
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.Test.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
                .ForMember(dest => dest.Test, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());

            CreateMap<TestQuestion, DbLogic.TestQuestion>();
            CreateMap<AnswerOption, DbLogic.AnswerOption>();

            CreateMap<StudentTestTaker, DbLogic.Student>();

            CreateMap<DbLogic.Test, Test>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
            CreateMap<DbLogic.PassedTest, PassedTest>()
                .ForMember(dest => dest.Student, opt => opt.Ignore());
            CreateMap<DbLogic.TestQuestion, TestQuestion>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.AnswerOptions));
            CreateMap<DbLogic.AnswerOption, AnswerOption>();

            CreateMap<DbLogic.Student, StudentTestTaker>();
        }
    }
}
