using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.appCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GliglockTest.appCore.Account;

namespace GliglockTest.Controllers
{
    public class TestsController : Controller
    {
        private readonly TestsDbContext _dbContext;
        private readonly IMapper _mapper;
        private static List<appCore.Test>? _tests;

        private async Task RefillLocalTests()
        {
            var allTestsDb = await _dbContext.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
            _tests = _mapper.Map<List<appCore.Test>>(allTestsDb);
        }
        public TestsController(TestsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var allTestsDb = await _dbContext.Tests
                .Include(t=>t.Teacher)
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
            _tests = _mapper.Map<List<appCore.Test>>(allTestsDb);
            return View(_tests);
        }

        public async Task<IActionResult> CertainTest(Guid testId)
        {
            if (_tests == null)
            {
                RefillLocalTests();
            }
            var test = _tests.First(t => t.Id == testId);
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<Answer> answers, [FromQuery] Guid TestId)
        {
            appCore.PassedTest passedTest = new appCore.PassedTest();

            passedTest.Test = _tests.First(t => t.Id == TestId);
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
                appCore.Account.StudentTestTaker student = new(_dbContext, _mapper);
                var studentDb = await _dbContext.Students
                    .Include(s => s.PassedTests)
                    .ThenInclude(pt => pt.Test)
                    .FirstAsync(s => s.Email == User.Identity.Name);
                student.Id = studentDb.Id;
                student.Email = studentDb.Email;
                student.FirstName = studentDb.FirstName;
                student.LastName = studentDb.LastName;
                student.BirthDay = studentDb.BirthDay;
                var studentsPassedTests = _mapper.Map<List<appCore.PassedTest>>(studentDb.PassedTests);
                student.PassedTests = studentsPassedTests;
                await student.SaveTestResultAsync(passedTest);
            }
            return View("Results", passedTest);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PassedTests()
        {
            var email = User.Identity.Name;
            var passedTestsDb = await _dbContext.PassedTests
                .Include(pt => pt.Test)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();

            var passedTests = _mapper.Map<List<appCore.PassedTest>>(passedTestsDb);

            return View(passedTests);
        }

        public async Task<IActionResult> CreatedTests()
        {
            var teacherDb = await _dbContext.Teachers.FirstAsync(t => t.Email == User.Identity.Name);
            if (_tests == null)
            {
                await RefillLocalTests();
            }
            var theirTestIds = _tests
                .Where(t => t.TeacherId == teacherDb.Id)
                .Select(t => t.Id)
                .ToList();
            var passedTestsDb = await _dbContext.PassedTests
                .Include(pt => pt.Student)
                .Include(pt => pt.Test)
                .Where(pt => theirTestIds
                .Contains(pt.TestId))
                .ToListAsync();
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
            if (ModelState.IsValid)
            {
                if (testModel == null)
                {
                    throw new ArgumentNullException(nameof(testModel));
                }
                if (testModel.Questions.Any(q => q.AnswerOptions.Count(ao => ao.IsCorrect) == 0))
                {
                    ModelState.AddModelError("", "Each Question must has at least one correct answer");
                    return View(testModel);
                }
                testModel.Questions.ForEach(q => q.WithImg = UploadImage(q.Image, q.Id.ToString()));


                var teacherDb = await _dbContext.Teachers.FirstAsync(t => t.Email == User.Identity.Name);
                TeacherTestCreator teacher = new TeacherTestCreator(_dbContext, _mapper, teacherDb);
                await teacher.CreateTest(testModel);
                await RefillLocalTests();
                return RedirectToAction("Index");
            }
            else
            {
                return View(testModel);
            }

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
