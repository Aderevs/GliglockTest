using GliglockTest.appCore.Account;
using GliglockTest.DbLogic;
using GliglockTest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace GliglockTest.Controllers
{
    public class AccountController : Controller
    {
        private TestsDbContext _dbContext;

        public AccountController(TestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
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
                bool loginIsUnique = !_dbContext.Students.Any(u => u.Email == model.Email);
                if (loginIsUnique)
                {
                    var newUser = new Student
                    {
                        Id = Guid.NewGuid(),
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDay = model.Birthday,
                        Salt = Guid.NewGuid(),
                    };
                    newUser.PasswordHash = PasswordHasher.HashPassword(model.Password + newUser.Salt.ToString());
                    _dbContext.Students.Add(newUser);
                    await _dbContext.SaveChangesAsync();
                    await SignInAsync(newUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Email), "This Email is already in use");
                    return View(model);
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
                var userOrNull = await _dbContext.Students.FirstOrDefaultAsync(x => x.Email == model.Email);
                
                if (userOrNull is Student student)
                {
                    var isCorrectPassword = PasswordHasher.IsCorrectPassword(student, model.Password);
                    if (isCorrectPassword)
                    {
                        await SignInAsync(student);
                        return RedirectToAction("Index", "Tests");
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
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Student"),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }
    }
}
