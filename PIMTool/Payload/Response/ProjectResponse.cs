namespace PIMTool.Payload.Response;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Customer { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Version { get; set; }

    public ProjectResponse()
    {
    }

    public ProjectResponse(int id, string name, string customer, DateTime startDate, DateTime endDate, decimal version)
    {
        Id = id;
        Name = name;
        Customer = customer;
        StartDate = startDate;
        EndDate = endDate;
        Version = version;
    }
}