// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        #region Fields, Dependency injection
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        #endregion

        /// <summary>
        /// Property koji se koristi za rad sa FORM dijelom Razor pagea za registraciju
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Klasa koja se koristi za rad sa FORM dijelom Razor pagea za registraciju
        /// </summary>
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Novi elementi na ekranu za registraciju korisnika:
            [Required]
            [StringLength(20, ErrorMessage = $"First name must be at least 1 and at max 20 characters long.", MinimumLength = 1)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = $"Last name must be at least 1 and at max 20 characters long.", MinimumLength = 1)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date of birth")]
            public DateOnly DateOfBirth { get; set; }
        }

        #region Methods
        /// <summary>
        /// GET metoda
        /// </summary>
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// POST metoda
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                //Kod kreacije User instance ne postavljaju se vrijednosti novih polja?
                var user = CreateUser();

                //Rješenje: RUČNO postaviti vrijednosti novih polja u instancu:
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.DateOfBirth = Input.DateOfBirth;

                //Postavljanje korisničkog imena (username je zapravo email adresa)
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

                //Postavljanje emaila:
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                //Kreiranje novog korisnika sa lozinkom (nakon validacije svih podataka o korisniku):
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    //Nakon uspješne registracije, korisnika se preusmjerava na stranicu za potvrdu email adrese:
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //Slanje maila za potvrdu:
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else //Ako potvrda email adrese nije potrebna onda korisnika samo ulogiraj:
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        //Ako smo prije registracije bili na nekoj stranici unutar aplikacije onda nas se vraća na tu stranicu:
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors) //Ako je prekršena neka validacija:
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        /// <summary>
        /// Metoda koja dodaje novog korisnika u tablicu na bazi ako su podaci OK
        /// </summary>
        private ApplicationUser CreateUser()
        {
            try
            {
                //Instanciranje data-modela ApplicationUser:
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
        #endregion
    }
}
