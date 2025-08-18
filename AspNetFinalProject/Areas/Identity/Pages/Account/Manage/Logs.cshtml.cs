using AspNetFinalProject.Common;
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
    
    public PagedResult<UserActionLogDto>? UserLogs { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int PageIndex { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;
    
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var request = new PagedRequest
        {
            Page = PageIndex,
            PageSize = PageSize,
        };
        
        UserLogs = (await _userActionLogService.GetByUserIdAsync(user.Id, request))
            .Map(UserActionLogMapper.CreateDto);
        return Page();
    }
}