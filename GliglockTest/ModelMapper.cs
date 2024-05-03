using AutoMapper;

namespace GliglockTest
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

            CreateMap<DbLogic.Test, Test>();
            CreateMap<DbLogic.PassedTest, PassedTest>();
            CreateMap<DbLogic.TestQuestion, TestQuestion>();
            CreateMap<DbLogic.AnswerOption, AnswerOption>();
        }
    }
}
