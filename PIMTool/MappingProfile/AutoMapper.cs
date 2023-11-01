using AutoMapper;
using PIMTool.Entities;
using PIMTool.Payload.Request.Service;
using PIMTool.Payload.Response;

namespace PIMTool.MappingProfile;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        MapProject();   
        MapEmployee();
        MapGroup();
    }

    private void MapProject()
    {
        //Response
        CreateMap<ProjectEntity, ProjectResponse>()
            .ReverseMap();
    }

    private void MapEmployee()
    {
        //Response
        CreateMap<EmployeeEntity, EmployeeResponse>()
            .ReverseMap();
        //Create employee request
        CreateMap<CEmployeeRequest, EmployeeEntity>()
            .ReverseMap();
        //Delete employee request
        CreateMap<DEmployeeRequest, EmployeeEntity>()
            .ReverseMap();
        CreateMap<UEmployeeRequest, EmployeeEntity>()
            .ReverseMap();
    }

    private void MapGroup()
    {
        //Response
        CreateMap<GroupEntity, GroupResponse>()
            .ReverseMap();
    }
}