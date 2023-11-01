using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Extensions;
using PIMTool.Payload.Request.Authentication;
using PIMTool.Payload.Request.Paging;
using PIMTool.Payload.Response;
using PIMTool.Services;

namespace PIMTool.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProjectService _projectService;
    private readonly IAuthenticationService _authenticationService;
    private IAuthorizationService _authorizationService;

    public ProjectController(IMapper mapper, IProjectService projectService, IAuthenticationService authenticationService, IAuthorizationService authorizationService)
    {
        _mapper = mapper;
        _projectService = projectService;
        _authenticationService = authenticationService;
        _authorizationService = authorizationService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjects();
        if (!projects.Any())
        {
            return NotFound(new BaseResponse(404, "Not Found!", null));
        }

        return Ok(new BaseResponse(200, "Successful!", _mapper.Map<List<ProjectResponse>>(projects)));
    }

    [HttpGet("get-all-with-paging/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetAllProjectsWithPaging(int currentPage, int pageSize)
    {
        var projects = await _projectService.GetAllProjectsWithPaging(new PagingParameter(currentPage, pageSize));
        return (!projects.Any())
            ? NotFound(new BaseResponse(404, "Not Found!", null))
            : Ok(new BaseResponse(200, "Successful!", projects));
    }
    
    [HttpGet("test-authorization")]
    public async Task<IActionResult> TestAuthorization()
    {
        if (_authenticationService.IsAccessible(_authenticationService.GetRole(User).Value, "Admin"))
        {
            return Ok(new BaseResponse(200, "You can view this action!", null));
        }
        return Unauthorized(new BaseResponse(401, "You don't have permission to view this action!", null));
    }
}