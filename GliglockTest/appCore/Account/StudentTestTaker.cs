using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.DbLogic.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest.appCore.Account
{
    public class StudentTestTaker(IMapper mapper, IPassedTestsRepository passedTestsRepository) : BaseUser
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPassedTestsRepository _passedTestsRepository = passedTestsRepository;
        //private readonly DbModelAdapter _modelAdapter;
        public List<PassedTest>? PassedTests { get; set; }

        public async Task SaveTestResultAsync(PassedTest completedTest)
        {
            if (completedTest == null)
            {
                throw new ArgumentNullException(nameof(completedTest));
            }
            completedTest.Student = _mapper.Map<Models.StudentView>(this);
            PassedTests.Add(completedTest);

            var dbPassedTest = _mapper.Map<DbLogic.PassedTest>(completedTest);
            await _passedTestsRepository.AddPassedTestAsync(dbPassedTest);
        }

        public void ParseStudentFromDb(DbLogic.Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            Id = student.Id;
            Email = student.Email;
            BirthDay = student.BirthDay;
            FirstName = student.FirstName;
            LastName = student.LastName;
            if (student.PassedTests != null && student.PassedTests.Any())
            {
                PassedTests = _mapper.Map<List<PassedTest>>(student.PassedTests);
            }
        }

    }
}
