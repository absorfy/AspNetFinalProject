using AspNetFinalProject.DTOs;
using AspNetFinalProject.Mappers;
using AspNetFinalProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetFinalProject.Areas.Identity.Pages.Account.Manage;

public class Logs : PageModel
{
    private readonly IUserActionLogService _userActionLogService;
    private readonly UserManager<IdentityUser> _userManager;
    
    
    public Logs(
        IUserActionLogService userActionLogService,
        UserManager<IdentityUser> userManager)
    {
        _userActionLogService = userActionLogService;
        _userManager = userManager;
    }
    
    public IEnumerable<UserActionLogDto> UserLogs { get; set; }
    
    
    
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }
        
        var logs = await _userActionLogService.GetByUserIdAsync(user.Id);
        UserLogs = logs.Select(UserActionLogMapper.CreateDto);
        return Page();
    }
}