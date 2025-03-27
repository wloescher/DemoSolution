using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("WorkItemUser")]
[Index("WorkItemUserWorkItemId", "WorkItemUserUserId", Name = "UQ_WorkItemUser", IsUnique = true)]
public partial class WorkItemUser
{
    [Key]
    public int WorkItemUserId { get; set; }

    public int WorkItemUserWorkItemId { get; set; }

    public int WorkItemUserUserId { get; set; }

    public bool WorkItemUserIsDeleted { get; set; }

    [InverseProperty("WorkItemUserAuditWorkItemUser")]
    public virtual ICollection<WorkItemUserAudit> WorkItemUserAudits { get; set; } = new List<WorkItemUserAudit>();

    [ForeignKey("WorkItemUserUserId")]
    [InverseProperty("WorkItemUsers")]
    public virtual User WorkItemUserUser { get; set; }

    [ForeignKey("WorkItemUserWorkItemId")]
    [InverseProperty("WorkItemUsers")]
    public virtual WorkItem WorkItemUserWorkItem { get; set; }
}
