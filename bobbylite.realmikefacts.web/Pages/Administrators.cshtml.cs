using System.Globalization;
using System.Text.Json;
using Ardalis.GuardClauses;
using bobbylite.realmikefacts.web.Constants;
using bobbylite.realmikefacts.web.Models.AccessRequest;
using bobbylite.realmikefacts.web.Services.AccessRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace bobbylite.realmikefacts.web.Pages;

/// <summary>
/// Administrators page model.
/// </summary>
[Authorize(Policy = PolicyNames.AdministratorsGroup)]
[Authorize(Policy = PolicyNames.Users)]
public class AdministratorsModel : PageModel
{
    private readonly ILogger<AdministratorsModel> _logger;
    private readonly AccessRequestService _accessRequestService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AdministratorsModel"/> class.
    /// </summary>
    /// <param name="logger">Logger from DI.</param>
    /// <param name="accessRequestService"></param>
    public AdministratorsModel(ILogger<AdministratorsModel> logger, IAccessRequestService accessRequestService)
    {
        _logger = Guard.Against.Null(logger);
        Guard.Against.Null(accessRequestService);
        
        _accessRequestService = (AccessRequestService)accessRequestService;
    }

    /// <summary>
    /// 
    /// </summary>
    [BindProperty]
    public string? Data { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IDictionary<string, AccessRequestModel> GetAllRequests()
    {
        return _accessRequestService.GetAllAccessRequests();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostApprove()
    {
        var requests = _accessRequestService.GetAllAccessRequests();
        
        if (string.IsNullOrEmpty(Data) || Data is null)
        {
             throw new NullOrEmptyStringException(nameof(Data));
        }

        var data = JsonSerializer.Deserialize<AccessRequestModel>(Data);

        if (data is null)
        { 
            throw new NullObjectException(nameof(data));
        }

        foreach(var request in requests.Values.Where(r => r.Id == data.Id))
        {
            await _accessRequestService.FulfillRequest(request.Id!, request.RequestedResource!);
        }

        return Page();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IActionResult OnPostDeny()
    {
        return Page();
    }
}