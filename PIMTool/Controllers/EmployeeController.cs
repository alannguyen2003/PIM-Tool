using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Entities;
using PIMTool.Payload.Request.Paging;
using PIMTool.Payload.Request.Service;
using PIMTool.Payload.Response;
using PIMTool.Services;

namespace PIMTool.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IMapper mapper, IEmployeeService employeeService)
    {
        _mapper = mapper;
        _employeeService = employeeService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllEmployee()
    {
        var employees = await _employeeService.GetAllEmployees();
        if (!employees.Any())
        {
            return NotFound(new BaseResponse(404, "Not Found!", null));
        }
        return Ok(new BaseResponse(200, "Successful!", _mapper.Map<List<EmployeeResponse>>(employees.OrderBy(emp => emp.FirstName))));
    }
    
    [HttpGet("get-all-with-paging/{currentPage}/{pageSize}")]
    public async Task<IActionResult> GetAllEmployeesWithPaging(int currentPage, int pageSize)
    {
        var employees = await _employeeService.GetAllEmployeesWithPaging(new PagingParameter(currentPage, pageSize));
        if (!employees.Any())
        {
            return NotFound(new BaseResponse(404, "Not Found!", null));
        }
        return Ok(new BaseResponse(200, "Successful!", _mapper.Map<List<EmployeeResponse>>(employees.OrderBy(emp => emp.FirstName))));
    }

    [HttpPost("insert")]
    public async Task<IActionResult> InsertNewEmployee(CEmployeeRequest request)
    {
        try
        {
            await _employeeService.AddNewEmployee(_mapper.Map<CEmployeeRequest, EmployeeEntity>(request));
            return Created("", new BaseResponse(201, "Add successful!", null));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse(400, request.ToString(), null));
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            DEmployeeRequest request = new DEmployeeRequest();
            request.Id = id;
            await _employeeService.DeleteEmployee(_mapper.Map<DEmployeeRequest, EmployeeEntity>(request));
            return Ok(new BaseResponse(204, "Delete successfully!", null));
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse(400, "Delete unsuccessfully!", null));
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> ModifyEmployee(UEmployeeRequest request)
    {
        var employee = await _employeeService.GetEmployeeById(request.Id);
        var updateEmployee = _mapper.Map<UEmployeeRequest, EmployeeEntity>(request);
        try
        {
            if (employee.Version < updateEmployee.Version + 1)
            {
                updateEmployee.Version = updateEmployee.Version + 1;
                await _employeeService.UpdateEmployee(updateEmployee);
                return Ok(new BaseResponse(200, "Modify successfully!", null));
            }
            else
            {
                return BadRequest(new BaseResponse(400, "Modify unsuccessful!", null));
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseResponse(400, "Fault!", ex.Message));
        }
    }
}