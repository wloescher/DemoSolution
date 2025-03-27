using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("WorkItemUserAudit")]
public partial class WorkItemUserAudit
{
    [Key]
    public int WorkItemUserAuditId { get; set; }

    public int WorkItemUserAuditActionId { get; set; }

    public int WorkItemUserAuditWorkItemUserId { get; set; }

    public int WorkItemUserAuditUserId { get; set; }

    public DateTime WorkItemUserAuditDate { get; set; }

    [Required]
    public string WorkItemUserAuditBeforeJson { get; set; }

    [Required]
    public string WorkItemUserAuditAfterJson { get; set; }

    [Required]
    public string WorkItemUserAuditAffectedColumns { get; set; }

    [ForeignKey("WorkItemUserAuditActionId")]
    [InverseProperty("WorkItemUserAudits")]
    public virtual DataDictionary WorkItemUserAuditAction { get; set; }

    [ForeignKey("WorkItemUserAuditUserId")]
    [InverseProperty("WorkItemUserAudits")]
    public virtual User WorkItemUserAuditUser { get; set; }

    [ForeignKey("WorkItemUserAuditWorkItemUserId")]
    [InverseProperty("WorkItemUserAudits")]
    public virtual WorkItemUser WorkItemUserAuditWorkItemUser { get; set; }
}
