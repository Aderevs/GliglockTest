using AutoMapper;
using System.Linq.Expressions;

namespace GliglockTest.appCore
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<Test, DbLogic.Test>();
            CreateMap<PassedTest, DbLogic.PassedTest>()
                .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.Test.Id))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id));

            CreateMap<TestQuestion, DbLogic.TestQuestion>();
            CreateMap<AnswerOption, DbLogic.AnswerOption>();

            CreateMap<DbLogic.Test, Test>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
            CreateMap<DbLogic.PassedTest, PassedTest>();
            CreateMap<DbLogic.TestQuestion, TestQuestion>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.AnswerOptions));
            CreateMap<DbLogic.AnswerOption, AnswerOption>();
        }
    }
}
