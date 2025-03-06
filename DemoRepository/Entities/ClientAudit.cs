using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("ClientAudit")]
public partial class ClientAudit
{
    [Key]
    public int ClientAuditId { get; set; }

    public int ClientAuditActionId { get; set; }

    public int ClientAuditClientId { get; set; }

    public int ClientAuditUserId { get; set; }

    public DateTime ClientAuditDate { get; set; }

    [Required]
    public string ClientAuditBeforeJson { get; set; }

    [Required]
    public string ClientAuditAfterJson { get; set; }

    [Required]
    public string ClientAuditAffectedColumns { get; set; }

    [ForeignKey("ClientAuditActionId")]
    [InverseProperty("ClientAudits")]
    public virtual DataDictionary ClientAuditAction { get; set; }

    [ForeignKey("ClientAuditClientId")]
    [InverseProperty("ClientAudits")]
    public virtual Client ClientAuditClient { get; set; }

    [ForeignKey("ClientAuditUserId")]
    [InverseProperty("ClientAudits")]
    public virtual User ClientAuditUser { get; set; }
}
