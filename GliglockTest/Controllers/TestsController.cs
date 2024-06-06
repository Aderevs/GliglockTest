using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.appCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using GliglockTest.DbLogic.Repositories.Interfaces;
using GliglockTest.Models.Bindings;
using GliglockTest.appCore.Interfaces;

namespace GliglockTest.Controllers
{
    public class TestsController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IStudentsRepository _studentsRepository;
        private readonly ITeachersRepository _teachersRepository;
        private readonly IPassedTestsRepository _passedTestsRepository;

        private readonly StudentTestTaker _student;
        private readonly TeacherTestCreator _teacher;

        private readonly ICacheProvider _cacheProvider;

        public TestsController(
                   IMapper mapper,
                   IStudentsRepository studentsRepository,
                   ITeachersRepository teachersRepository,
                   IPassedTestsRepository passedTestsRepository,
                   StudentTestTaker student,
                   TeacherTestCreator teacher,
                   ICacheProvider cacheProvider)
        {
            _mapper = mapper;
            _studentsRepository = studentsRepository;
            _passedTestsRepository = passedTestsRepository;
            _teachersRepository = teachersRepository;
            _student = student;
            _teacher = teacher;
            _cacheProvider = cacheProvider;
        }

        public async Task<IActionResult> Index()
        {
            TestsListPage? testsListPage = await _cacheProvider.GetTestsForPageAsync();
            return View(testsListPage);

        }

        public async Task<IActionResult> CertainTest(Guid testId)
        {
            appCore.Test test = await _cacheProvider.GetCertainTestAsync(testId);
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<AnswerBindingModel> answers, [FromQuery] Guid testId)
        {
            appCore.PassedTest passedTest = new appCore.PassedTest();
            appCore.Test test = await _cacheProvider.GetCertainTestAsync(testId);
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
                if (_student.Email == null || _student.Email != User.Identity.Name)
                {
                    var studentDb = await _studentsRepository.GetStudentByEmailIncludePassedTestsAsync(User.Identity.Name);
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
            var passedTestsDb = await _passedTestsRepository.GetPassedTestsOfStudentByEmailAsync(email);

            var passedTests = _mapper.Map<List<appCore.PassedTest>>(passedTestsDb);

            return View(passedTests);
        }

        public async Task<IActionResult> CreatedTests()
        {
            var teacherDb = await _teachersRepository.GetTeacherByEmailAsync(User.Identity.Name);
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


            
            if (_teacher.Email == null || _teacher.Email != User?.Identity?.Name)
            {
                var teacherDb = await _teachersRepository.GetTeacherByEmailIncludeCreatedTestsAsync(User.Identity.Name);
                _teacher.ParseTeacherFromDb(teacherDb);
            }
            await _teacher.CreateTest(testModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TestsList(ushort page)
        {
            TestsListPage testsListPage = await _cacheProvider.GetTestsForPageAsync(page);
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
