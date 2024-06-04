using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.appCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GliglockTest.appCore.Account;
using Microsoft.Extensions.Caching.Memory;
using GliglockTest.DbLogic.Repositories.Interfaces;

namespace GliglockTest.Controllers
{
    public class TestsController : Controller
    {
        private readonly TestsDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ITestsRepository _testsRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITeachersRepository _teachersRepository;
        private readonly IPassedTestsRepository _passedTestsRepository;

        private StudentTestTaker _student;
        private TeacherTestCreator _teacher;

        private readonly IMemoryCache _cache;
        private const string CacheKeyTestsList = "TestsListPage";
        private const string CacheKeyCertainTest = "Tests";
        private const int NumberOfTestsInOnePage = 2;

        public TestsController(
                   TestsDbContext dbContext,
                   IMapper mapper,
                   IMemoryCache memoryCache,
                   ITestsRepository testRepository,
                   IStudentsRepository studentsRepository,
                   ITeachersRepository teachersRepository,
                   IPassedTestsRepository passedTestsRepository,
                   StudentTestTaker student,
                   TeacherTestCreator teacher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cache = memoryCache;
            _testsRepository = testRepository;
            _studentsRepository = studentsRepository;
            _passedTestsRepository = passedTestsRepository;
            _teachersRepository = teachersRepository;
            _student = student;
            _teacher = teacher;
            /*
                        if (User.IsInRole("Student") &&
                            (_student.Email == null || _student.Email != User?.Identity?.Name))
                        {
                            var studentDb = _studentsRepository.GetStudentByEmailIncludePassedTests(User.Identity.Name);
                            _student.ParseStudentFromDb(studentDb);
                        }
                        else if (User.IsInRole("Teacher") &&
                            (_teacher.Email == null || _teacher.Email != User?.Identity?.Name))
                        {
                            var teacherDb = _teachersRepository.GetTeacherByEmailIncludeCreatedTests(User.Identity.Name);
                            _teacher.ParseTeacherFromDb(teacherDb);
                        }*/
        }


        private async Task<TestsListPage> RefillCacheForPageAndGetTests(ushort pageNumber)
        {
            var tests = await FetchTestsPageFromDB(pageNumber);
            TestsListPage testsList = new TestsListPage
            {
                TestList = tests,
                PageNumber = pageNumber
            };
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(CacheKeyTestsList + pageNumber.ToString(), testsList, cacheEntryOptions);
            return testsList;
        }
        private async Task<TestsListPage> RefillCacheForPageAndGetTests() => await RefillCacheForPageAndGetTests(1);

        private async Task<appCore.Test> GetCertainTestAndAddToCache(Guid testId)
        {
            var test = await FetchTestFromDb(testId);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
               .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            _cache.Set(CacheKeyCertainTest + testId.ToString(), test, cacheEntryOptions);
            return test;
        }
        private async Task<List<appCore.Test>> FetchTestsPageFromDB(int pageNumber)
        {
            /*var allTestsDb = await _dbContext.Tests
                .Include(t => t.Teacher)
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .OrderBy(t => t.Name)
                .Skip((pageNumber - 1) * NumberOfTestsInOnePage)
                .Take(NumberOfTestsInOnePage)
                .ToListAsync();*/

            var allTestsDb = await _testsRepository.GetPaginatedSolidTestsIncludeTeacherAsync(NumberOfTestsInOnePage, pageNumber);
            return _mapper.Map<List<appCore.Test>>(allTestsDb);
        }

        private async Task<appCore.Test> FetchTestFromDb(Guid testId)
        {
            /*var testDb = await _dbContext.Tests
                    .Include(t => t.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                    .FirstAsync(t => t.Id == testId);*/

            var testDb = await _testsRepository.GetSolidTestByIdAsync(testId);
            var test = _mapper.Map<appCore.Test>(testDb);
            return test;
        }


        public async Task<IActionResult> Index()
        {
            TestsListPage? testsListPage;
            if (!_cache.TryGetValue(CacheKeyTestsList + "1", out testsListPage))
            {
                testsListPage = await RefillCacheForPageAndGetTests();
            }
            return View(testsListPage);

        }

        public async Task<IActionResult> CertainTest(Guid testId)
        {
            appCore.Test test;
            if (!_cache.TryGetValue(CacheKeyCertainTest + testId.ToString(), out test))
            {
                test = await GetCertainTestAndAddToCache(testId);
            }
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<Answer> answers, [FromQuery] Guid testId)
        {
            appCore.PassedTest passedTest = new appCore.PassedTest();
            appCore.Test test;
            if (!_cache.TryGetValue(CacheKeyCertainTest + testId.ToString(), out test))
            {
                test = await GetCertainTestAndAddToCache(testId);
            }
            passedTest.Test = test;
            if (answers == null)
            {
                passedTest.Mark = 0;
                Console.WriteLine("NULL");
                return View("Results", passedTest);
            }

            passedTest.Test.Questions.ForEach(question =>
            {
                question.ClearStudentAnswer();
                var answersToSet = question.AnswerOptions;
                var currentQuestionAnswers = answers
                .First(a => a.QuestionId == question.Id)
                .SelectedOptions;

                answersToSet = answersToSet
                .Where(ats => currentQuestionAnswers.Contains(ats.Id))
                .ToList();
                question.AddStudentsAnswer(answersToSet);
            });
            passedTest.CalculateMark();
            if (User.Identity.IsAuthenticated)
            {
                //appCore.Account.StudentTestTaker student = new(_mapper);
                /*var studentDb = await _dbContext.Students
                    .Include(s => s.PassedTests)
                    .ThenInclude(pt => pt.Test)
                    .FirstAsync(s => s.Email == User.Identity.Name);*/
                /*var studentDb = await _studentsRepository.GetStudentByEmailIncludePassedTestsAsync(User.Identity.Name);
                student.Id = studentDb.Id;
                student.Email = studentDb.Email;
                student.FirstName = studentDb.FirstName;
                student.LastName = studentDb.LastName;
                student.BirthDay = studentDb.BirthDay;
                var studentsPassedTests = _mapper.Map<List<appCore.PassedTest>>(studentDb.PassedTests);
                student.PassedTests = studentsPassedTests;*/
                if (_student.Email == null || _student.Email != User.Identity.Name)
                {
                    var studentDb = _studentsRepository.GetStudentByEmailIncludePassedTests(User.Identity.Name);
                    _student.ParseStudentFromDb(studentDb);
                }
                await _student.SaveTestResultAsync(passedTest);
            }
            return View("Results", passedTest);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PassedTests()
        {
            var email = User?.Identity?.Name;
            /*var passedTestsDb = await _dbContext.PassedTests
                .Include(pt => pt.Student)
                .Where(pt => pt.Student.Email == email)
                .Include(pt => pt.Test)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();*/
            var passedTestsDb = await _passedTestsRepository.GetPassedTestsOfStudentByEmailAsync(email);

            var passedTests = _mapper.Map<List<appCore.PassedTest>>(passedTestsDb);

            return View(passedTests);
        }

        public async Task<IActionResult> CreatedTests()
        {
            var teacherDb = await _teachersRepository.GetTeacherByEmailAsync(User.Identity.Name);
            //_dbContext.Teachers.FirstAsync(t => t.Email == User.Identity.Name);
            /*var theirTestIds = await _dbContext.Tests
                .Where(t => t.TeacherId == teacherDb.Id)
                .Select(t => t.Id)
                .ToListAsync();
            var passedTestsDb = await _dbContext.PassedTests
                .Include(pt => pt.Student)
                .Include(pt => pt.Test)
                .Where(pt => theirTestIds.Contains(pt.TestId))
                .ToListAsync();*/
            var passedTestsDb = await _passedTestsRepository.GetPassedTestsOfTeachersCreatedByIdAsync(teacherDb.Id);
            var passedTests = _mapper.Map<List<appCore.PassedTest>>(passedTestsDb);
            return View(passedTests);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateTest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest([FromForm] appCore.Test testModel)
        {

            if (testModel == null)
            {
                throw new ArgumentNullException(nameof(testModel));
            }
            if (testModel.MaxMark == 0)
            {
                testModel.MaxMark = 100;
            }
            if (testModel.Questions.Any(q => q.AnswerOptions.Count(ao => ao.IsCorrect) == 0))
            {
                ModelState.AddModelError("", "Each Question must has at least one correct answer");
                return View(testModel);
            }
            testModel.Questions.ForEach(q => q.WithImg = UploadImage(q.Image, q.Id.ToString()));


            /*var teacherDb = await _teachersRepository.GetTeacherByEmailAsync(User.Identity.Name);
            //_dbContext.Teachers.FirstAsync(t => t.Email == User.Identity.Name);
            TeacherTestCreator teacher = new TeacherTestCreator(_mapper, teacherDb);*/
            if (_teacher.Email == null || _teacher.Email != User?.Identity?.Name)
            {
                var teacherDb = _teachersRepository.GetTeacherByEmailIncludeCreatedTests(User.Identity.Name);
                _teacher.ParseTeacherFromDb(teacherDb);
            }
            await _teacher.CreateTest(testModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TestsList(ushort page)
        {
            TestsListPage testsListPage;
            if (!_cache.TryGetValue(CacheKeyTestsList + page.ToString(), out testsListPage))
            {
                testsListPage = await RefillCacheForPageAndGetTests(page);
            }
            return View("Index", testsListPage);
        }
        private bool UploadImage(IFormFile image, string name)
        {
            if (image == null) return false;

            var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "questionImages");

            if (!Directory.Exists(ImagesFolder))
            {
                Directory.CreateDirectory(ImagesFolder);
            }

            var filePath = Path.Combine(ImagesFolder, name + ".jpg");
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return true;
        }
    }
}
