using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("WorkItem")]
[Index("WorkItemClientId", "WorkItemTitle", Name = "UQ_WorkItemTitle", IsUnique = true)]
public partial class WorkItem
{
    [Key]
    public int WorkItemId { get; set; }

    public Guid WorkItemGuid { get; set; }

    public int WorkItemClientId { get; set; }

    public int WorkItemTypeId { get; set; }

    public int WorkItemStatusId { get; set; }

    public bool WorkItemIsActive { get; set; }

    public bool WorkItemIsDeleted { get; set; }

    [StringLength(150)]
    public string WorkItemTitle { get; set; }

    [StringLength(150)]
    public string WorkItemSubTitle { get; set; }

    [StringLength(500)]
    public string WorkItemSummary { get; set; }

    public string WorkItemBody { get; set; }

    [InverseProperty("WorkItemAuditWorkItem")]
    public virtual ICollection<WorkItemAudit> WorkItemAudits { get; set; } = new List<WorkItemAudit>();

    [ForeignKey("WorkItemClientId")]
    [InverseProperty("WorkItems")]
    public virtual Client WorkItemClient { get; set; }

    [ForeignKey("WorkItemStatusId")]
    [InverseProperty("WorkItemWorkItemStatuses")]
    public virtual DataDictionary WorkItemStatus { get; set; }

    [ForeignKey("WorkItemTypeId")]
    [InverseProperty("WorkItemWorkItemTypes")]
    public virtual DataDictionary WorkItemType { get; set; }
}
