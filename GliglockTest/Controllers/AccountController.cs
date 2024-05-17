﻿using GliglockTest.appCore.Account;
using GliglockTest.DbLogic;
using GliglockTest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace GliglockTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestsDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountController(TestsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Student"))
            {
                var studentDb = await _dbContext.Students.FirstAsync(s => s.Email == User.Identity.Name);
                var student = _mapper.Map<StudentView>(studentDb);
                return View(student);
            }
            else if (User.IsInRole("Teacher"))
            {
                var teacherDb = await _dbContext.Teachers.FirstAsync(t => t.Email == User.Identity.Name);
                var teacher = _mapper.Map<TeacherView>(teacherDb);
                return View(teacher);
            }
            else
            {
                throw new InvalidOperationException("Account Index. User with such role doesn't exist");
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegistrationBindingModel model)
        {
            if (ModelState.IsValid)
            {
                bool loginIsUnique;
                User newUser;
                if (!model.IsTeacher)
                {
                    loginIsUnique = !_dbContext.Students.Any(u => u.Email == model.Email);
                    if (loginIsUnique)
                    {
                        newUser = new Student
                        {
                            Id = Guid.NewGuid(),
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            BirthDay = model.Birthday,
                            Salt = Guid.NewGuid(),
                        };
                        newUser.PasswordHash = PasswordHasher.HashPassword(model.Password + newUser.Salt.ToString());
                        _dbContext.Students.Add((Student)newUser);
                        await _dbContext.SaveChangesAsync();
                        await SignInAsync(newUser);
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Email), "This Email is already in use");
                        return View(model);
                    }

                }
                else
                {
                    loginIsUnique = !_dbContext.Teachers.Any(u => u.Email == model.Email);
                    if (loginIsUnique)
                    {
                        newUser = new Teacher
                        {
                            Id = Guid.NewGuid(),
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            BirthDay = model.Birthday,
                            Salt = Guid.NewGuid(),
                        };
                        newUser.PasswordHash = PasswordHasher.HashPassword(model.Password + newUser.Salt.ToString());
                        _dbContext.Teachers.Add((Teacher)newUser);
                        await _dbContext.SaveChangesAsync();
                        await SignInAsync(newUser);
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Email), "This Email is already in use");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AuthenticationBindingModel model)
        {

            if (ModelState.IsValid)
            {
                User userOrNull;
                if (!model.IsTeacher)
                {
                    userOrNull = await _dbContext.Students.FirstOrDefaultAsync(x => x.Email == model.Email);
                    if (userOrNull is Student student)
                    {
                        var isCorrectPassword = PasswordHasher.IsCorrectPassword(student, model.Password);
                        if (isCorrectPassword)
                        {
                            await SignInAsync(student);
                            return RedirectToAction("Index", "Tests");
                        }
                    }
                }
                else
                {
                    userOrNull = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Email == model.Email);
                    if (userOrNull is Teacher teacher)
                    {
                        var isCorrectPassword = PasswordHasher.IsCorrectPassword(teacher, model.Password);
                        if (isCorrectPassword)
                        {
                            await SignInAsync(teacher);
                            return RedirectToAction("Index", "Tests");
                        }
                    }
                }


                ModelState.AddModelError("", "Wrong login or password");
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(User user)
        {
            string role;
            if (user is Student)
            {
                role = "Student";
            }
            else
            {
                role = "Teacher";
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }
    }
}
