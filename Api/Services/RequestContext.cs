using System.Security.Claims;
using Application.Common.Interfaces;

namespace Api.Services;

public class RequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string IpAddress => _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
    
    public Guid? UserId => 
        _httpContextAccessor?.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value is { } id 
            ? Guid.Parse(id) 
            : null;

    public string? Username => 
        _httpContextAccessor?.HttpContext?.User?
            .FindFirst(ClaimTypes.Name)?.Value;
}