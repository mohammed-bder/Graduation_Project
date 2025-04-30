using AutoMapper;
using Graduation_Project.Core;
using Graduation_Project.Core.Enums;
using Graduation_Project.Core.IServices;
using Graduation_Project.Core.Models.Identity;
using Graduation_Project.Core.Models.Pharmacies;
using Graduation_Project.Core.Models.SendingEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_Dashboard.MVC.Helpers;
using Pharmacy_Dashboard.MVC.ViewModel.Account;
using System.Security.Claims;

namespace Pharmacy_Dashboard.MVC.Controllers
{
    public class AccountController : Controller
    {
        #region Allow Dependancy Injection
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<AppUser> userManager ,
            SignInManager<AppUser> signInManager ,
            IUnitOfWork unitOfWork ,
            IMapper mapper ,
            IUserService userService ,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _emailService = emailService;
        }
        #endregion

        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var user = await _userService.UserExistsAsync(model.Email);
                if(user is null)
                {
                    // Create the user of type AppUser
                    var newUser = new AppUser
                    {
                        UserName = model.Email.Split("@")[0],
                        Email = model.Email,
                        FullName = model.Email.Split("@")[0],
                    };
                    var result = await _userManager.CreateAsync(newUser, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Add the user to the "Pharmacy" role
                    await _userManager.AddToRoleAsync(newUser, UserRoleType.Pharmacist.ToString());

                    // Create the pharmacy
                    var pharmacy = _mapper.Map<Pharmacy>(model);
                    pharmacy.ApplicationUserID = newUser.Id;

                    //// check pharamcy contact if not null add it 
                    //if(model.pharmacyContacts != null || model.pharmacyContacts.Any()  )
                    //{
                    //    foreach(var contact in model.pharmacyContacts)
                    //    {
                    //        pharmacy.pharmacyContacts.Add(new PharmacyContact
                    //        {
                    //            PhoneNumber = contact.PhoneNumber,
                    //        });
                    //    }
                    //}

                    // Add the pharmacy to the database
                    await _unitOfWork.Repository<Pharmacy>().AddAsync(pharmacy);
                    var checkResult = await _unitOfWork.CompleteAsync();
                    if(checkResult <= 0)
                    {
                        // Delete the user if pharmacy creation fails
                        await _userManager.DeleteAsync(newUser);

                        ModelState.AddModelError(string.Empty, "Failed to create pharmacy");

                        return View(model);
                    }

                    // Send a welcome email to the user
                    var emailBody = EmailTemplateService.GetWelcomeEmailBody(newUser.UserName, Url.Action("Login", "Account", null,  Request.Scheme));
                    var email = new Email()
                    {
                        Subject = "Welcome to our Pharmacy",
                        Recipients = model.Email,
                        Body = emailBody
                    };
                     await _emailService.SendEmailAsync(email);

                    return RedirectToAction(nameof(Login));
                }
                ModelState.AddModelError(string.Empty, "Email already exists");
            }
            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                // check this email is exist or not 
                var user = await _userService.UserExistsAsync(model.Email);
                if(user is not null)
                {
                    // check password
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                        if(result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login");
            }
            return View(model);
        }
        #endregion

        #region SignOut
        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Forget Password

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetLink(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userService.UserExistsAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user); // unique for this user once time
                    var passwordResetLink = Url.Action(nameof(ResetPassword), "Account", new { email = model.Email , token = token } , Request.Scheme);

                    // https://localhost:44303/Account/ResetPassword?email=mohammed@gmail.com&Token=sdfdsfasdfasdf

                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Recipients = model.Email,
                        Body = EmailTemplateService.GetResetPasswordEmailBody(passwordResetLink)
                    };

                    var flag = await _emailService.SendEmailAsync(email);
                    if(flag)
                        return RedirectToAction(nameof(CheckYourBox));

                    ModelState.AddModelError(string.Empty, "Failed to sending Email");
                    return View(model);
                }
                ModelState.AddModelError(string.Empty, "This Email not exist");
            }
            return View(nameof(ForgetPassword),model);
        }

        public IActionResult CheckYourBox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["Token"] as string;

                var user = await _userService.UserExistsAsync(email);
                if(user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if(result.Succeeded)
                        return RedirectToAction(nameof(Login));
                }
                ModelState.AddModelError(string.Empty, "Invalid reset password");
            }
            return View(model);
        }

        #endregion


    }
}
