using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Payload.Request.Paging;
using PIMTool.Payload.Response;
using PIMTool.Services;

namespace PIMTool.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IGroupService _groupService;

    public GroupController(IMapper mapper, IGroupService groupService)
    {
        _mapper = mapper;
        _groupService = groupService;
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _groupService.GetAllGroups();
        return (!groups.Any())
            ? NotFound(new BaseResponse(404, "Not Found!", null))
            : Ok(new BaseResponse(200, "Successful!", groups));
    }
    
    [HttpGet("get-all-with-paging/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetAllGroupsWithPaging(int currentPage, int pageSize)
    {
        var groups = await _groupService.GetAllGroupsWithPaging(new PagingParameter(currentPage, pageSize));
        return (!groups.Any())
            ? NotFound(new BaseResponse(404, "Not Found!", null))
            : Ok(new BaseResponse(200, "Successful!", groups));
    }
    
}