using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("WorkItemAudit")]
public partial class WorkItemAudit
{
    [Key]
    public int WorkItemAuditId { get; set; }

    public int WorkItemAuditActionId { get; set; }

    public int WorkItemAuditWorkItemId { get; set; }

    public int WorkItemAuditUserId { get; set; }

    public DateTime WorkItemAuditDate { get; set; }

    [Required]
    public string WorkItemAuditBeforeJson { get; set; }

    [Required]
    public string WorkItemAuditAfterJson { get; set; }

    [Required]
    public string WorkItemAuditAffectedColumns { get; set; }

    [ForeignKey("WorkItemAuditActionId")]
    [InverseProperty("WorkItemAudits")]
    public virtual DataDictionary WorkItemAuditAction { get; set; }

    [ForeignKey("WorkItemAuditUserId")]
    [InverseProperty("WorkItemAudits")]
    public virtual User WorkItemAuditUser { get; set; }

    [ForeignKey("WorkItemAuditWorkItemId")]
    [InverseProperty("WorkItemAudits")]
    public virtual WorkItem WorkItemAuditWorkItem { get; set; }
}
