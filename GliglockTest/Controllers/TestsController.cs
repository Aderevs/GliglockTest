﻿using AutoMapper;
using GliglockTest.DbLogic;
using GliglockTest.appCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace GliglockTest.Controllers
{
    public class TestsController : Controller
    {
        private readonly TestsDbContext _dbContext;
        private readonly IMapper _mapper;
        private static List<appCore.Test>? _tests;


        public TestsController(TestsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var allTestsDb = await _dbContext.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
            _tests = _mapper.Map<List<appCore.Test>>(allTestsDb);
            return View(_tests);
        }

        public async Task<IActionResult> CertainTest(Guid id)
        {
            if (_tests == null)
            {
                var allTestsDb = await _dbContext.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .ToListAsync();
                _tests = _mapper.Map<List<appCore.Test>>(allTestsDb);
            }
            var test = _tests.First(t => t.Id == id);
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
                var answersToSet = question.Options;
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
                    .Include(s=>s.PassedTests)
                    .ThenInclude(pt=>pt.Test)
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

        [Authorize]
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
    }


}
