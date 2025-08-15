// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AspNetFinalProject.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICurrentUserService _currentUserService;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _currentUserService = currentUserService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            
            [Required]
            [MaxLength(50)]
            [MinLength(3)]
            [Display(Name = "Username")]
            public string Username { get; set; }
            
            [MaxLength(50)]
            [Display(Name = "First Name")]
            public string Name { get; set; }

            [MaxLength(50)]
            [Display(Name = "Last Name")]
            public string Surname { get; set; }
            
            [Display(Name = "Birth Date")]
            [DataType(DataType.Date)]
            public DateTime? BirthDate { get; set; }

            [Display(Name = "Gender")]
            public GenderType Gender { get; set; }
            
            [Phone]
            [MaxLength(20)]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [MaxLength(500)]
            [Display(Name = "About")]
            public string About { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var profile = await _currentUserService.GetUserProfileAsync();
            if (profile == null)
            {
                throw new InvalidOperationException("Unable to load user profile.");
            }
            
            Username = profile.Username;

            Input = new InputModel
            {
                Username = profile.Username,
                Name = profile.PersonalInfo?.Name,
                Surname = profile.PersonalInfo?.Surname,
                BirthDate = profile.PersonalInfo?.BirthDate,
                Gender = profile.PersonalInfo?.Gender ?? GenderType.None,
                PhoneNumber = profile.PersonalInfo?.PhoneNumber,
                About = profile.PersonalInfo?.About,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var dto = new UpdateUserProfileDto
            {
                Username = Input.Username,
                PersonalInfo = new UpdatePersonalInfoDto
                {
                    Name = Input.Name,
                    Surname = Input.Surname,
                    BirthDate = Input.BirthDate,
                    Gender = (int)Input.Gender,
                    PhoneNumber = Input.PhoneNumber,
                    About = Input.About
                }
            };

            await _currentUserService.UpdateAsync(dto);
  
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
