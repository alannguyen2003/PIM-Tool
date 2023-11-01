using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PIMTool.Entities;

[Table("Projects")]
public class ProjectEntity : BaseEntity
{
    [Required]
    public int ProjectNumber { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = null!;

    [Required] [StringLength(50)] public string Customer { get; set; } = null!;
    
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    
    public int GroupId { get; set; }
    [ForeignKey("GroupId")] 
    public virtual GroupEntity Group { get; set; } = null!;
    
    [Required]
    [ConcurrencyCheck]
    public decimal Version { get; set; }

    
    public ProjectEntity()
    {
    }
}

internal class OnDeleteAttribute : Attribute
{
}