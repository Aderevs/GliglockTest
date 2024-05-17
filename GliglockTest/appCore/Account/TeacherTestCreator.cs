using AutoMapper;
using GliglockTest.DbLogic;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace GliglockTest.appCore.Account
{
    public class TeacherTestCreator : BaseUser
    {
        private readonly ITestBuilder _testBuilder;
        private readonly IMapper _mapper;
        private readonly DbModelAdapter _dbAdapter;
        public List<Test>? CreatedTests { get; set; }
        public TeacherTestCreator(TestsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            _testBuilder = new TestBuilder();
            _testBuilder.OnTestBuilt += CreateTest;
            _dbAdapter = new DbModelAdapter(dbContext, _mapper);
        }
        public TeacherTestCreator(TestsDbContext dbContext, IMapper mapper, DbLogic.Teacher teacherDb) : base(dbContext)
        {
            _mapper = mapper;
            _testBuilder = new TestBuilder();
            _testBuilder.OnTestBuilt += CreateTest;
            _dbAdapter = new DbModelAdapter(dbContext, _mapper);

            Id = teacherDb.Id;
            FirstName = teacherDb.FirstName;
            LastName = teacherDb.LastName;
            Email = teacherDb.Email;
            BirthDay = teacherDb.BirthDay;
        }
        public ITestBuilder CreateTestWithBuilder(string name)
        {
            _testBuilder.Clear();
            _testBuilder.AddName(name)
                .SetTeacherId(Id);
            return _testBuilder;
        }
        public async Task CreateTest(Test test)
        {
            test.TeacherId = Id;
            await _dbAdapter.SaveCreatedTestToDbAsync(test);
        }
    }
}
