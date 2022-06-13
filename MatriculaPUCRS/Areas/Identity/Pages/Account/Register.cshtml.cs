using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Entidades.Modelos;
using MatriculaPUCRS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Persistencia.Interfaces.Repositorios;
using MatriculaPUCRS.Areas.Roles;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace MatriculaPUCRS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IEstudanteRepositorio _estudanteRepositorio;
        private readonly ICurriculoRepositorio _curriculoRepositorio;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IEstudanteRepositorio estudanteRepositorio,
            ICurriculoRepositorio curriculoRepositorio)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _estudanteRepositorio = estudanteRepositorio;
            _curriculoRepositorio = curriculoRepositorio;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Nome")]
            [MinLength(4)]
            public string Nome { get; set; }

            [Required]
            [StringLength(11)]
            [RegularExpression(@"^(\d{11})$", ErrorMessage = "O CPF deve ter 11 dígitos.")]
            [Display(Name = "CPF")]
            public string CPF { get; set; }

            [Required]
            [StringLength(8, ErrorMessage = "A matrícula deve conter 8 dígitos.")]
            [RegularExpression(@"^[1-2][0-9][1-2]\d{5}$", ErrorMessage = "Código de matrícula inválido.")]
            [Display(Name = "Matricula (sem dígito verificador)")]
            public string Matricula { get; set; }

            [Required]
            [Display(Name = "Curriculo")]
            public long CurriculoID { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public SelectList Curriculos { get; set; }

        private async Task PopulateCurriculoDropDownList(object selectedCurriculo = null)
        {
            var curriculos = await _curriculoRepositorio.GetActiveCurriculosAsync();
            curriculos.OrderBy(c => c.NomeDoCurso).ThenBy(c => c.Codigo);

            Curriculos = new SelectList(curriculos, nameof(Curriculo.Id), nameof(Curriculo.NomeParaListaDropdown), selectedCurriculo);
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            await PopulateCurriculoDropDownList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                long matriculaId = Convert.ToInt64(Input.Matricula);
                Estudante estud = await _estudanteRepositorio.GetEntityById(matriculaId);

                if(estud is not null)
                {
                    ModelState.AddModelError(string.Empty, "ERRO: Matrícula já cadastrada no sistema.");
                    await PopulateCurriculoDropDownList(Input.CurriculoID);
                    return Page();
                }

                ApplicationUser user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    Estudante estudante = new Estudante() { 
                        Id = matriculaId,
                        CPF = Convert.ToUInt64(Input.CPF).ToString(@"000\.000\.000\-00"),
                        Nome = Input.Nome, 
                        Estado = EstadoEstudanteEnum.ATIVO, 
                        CurriculoId = Input.CurriculoID,
                    };

                    await _estudanteRepositorio.Add(estudante);
                    user.EstudanteId = estudante.Id;
                    await _userManager.AddToRoleAsync(user, Roles.Roles.Estudante.ToString()); // Adiciona o usuário a role Estudante.
                    await _userManager.UpdateAsync(user);

                    _logger.LogInformation("User created a new account with password.");
                    
                   
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            await PopulateCurriculoDropDownList(Input.CurriculoID);
            return Page();
        }
    }
}
