using GliglockTest.appCore.Account;
using GliglockTest.DbLogic;
using GliglockTest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;

namespace GliglockTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestsDbContext _dbContext;
        private readonly IMapper _mapper;
        //private readonly SignInManager<BaseUser> _signInManager;
        //private readonly UserManager<BaseUser> _userManager;
        private readonly ILogger<AccountController> _logger;


        public AccountController(
            TestsDbContext dbContext, 
            IMapper mapper,
            //SignInManager<BaseUser> signInManager,
           // UserManager<BaseUser> userManager,
            ILogger<AccountController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
           // _signInManager = signInManager;
           // _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var studentDb = await _dbContext.Students.FirstAsync(s => s.Email == User.Identity.Name);
            var student = _mapper.Map<StudentView>(studentDb);
            return View(student);
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
                return View(model);
            }
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(AuthenticationBindingModel model)
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

        /*public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Redirect to external authentication provider
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                // Handle external authentication provider error
                return RedirectToAction("SignIn");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Handle external authentication provider error
                return RedirectToAction("SignIn");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return LocalRedirect(returnUrl ?? "/");
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new RegistrationBindingModel { Email = email });
            }
        }*/

        public IActionResult ExternalLogin()
        {
            var prorerties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(prorerties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var identities = result.Principal.Identities.ToList();
            return Json(identities);
        }
    }
}
