using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace GliglockTest.appCore.Account
{
    public class TeacherTestCreator : BaseUser
    {
        private readonly ITestBuilder _testBuilder;
        private readonly IMapper _mapper;
        private readonly ITestsRepository _testsRepository;
        //private readonly DbModelAdapter _dbAdapter;
        public List<Test>? CreatedTests { get; set; }
        public TeacherTestCreator(IMapper mapper, ITestsRepository testsRepository)
        {
            _mapper = mapper;
            _testBuilder = new TestBuilder();
            _testBuilder.OnTestBuilt += CreateTest;
            _testsRepository = testsRepository;
            //_dbAdapter = new DbModelAdapter(dbContext, _mapper);
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
            //await _dbAdapter.SaveCreatedTestToDbAsync(test);
            var dbTest = _mapper.Map<DbLogic.Test>(test);
            await _testsRepository.AddTestAsync(dbTest);
        }
        public void ParseTeacherFromDb(DbLogic.Teacher teacher)
        {
            if (teacher == null)
            {
                throw new ArgumentNullException(nameof(teacher));
            }
            Id = teacher.Id;
            Email = teacher.Email;
            BirthDay = teacher.BirthDay;
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            if (teacher.Tests != null && teacher.Tests.Any())
            {
                CreatedTests = _mapper.Map<List<Test>>(teacher.Tests);
            }
        }
    }
}
