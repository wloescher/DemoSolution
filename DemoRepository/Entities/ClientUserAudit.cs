using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("ClientUserAudit")]
public partial class ClientUserAudit
{
    [Key]
    public int ClientUserAuditId { get; set; }

    public int ClientUserAuditActionId { get; set; }

    public int ClientUserAuditClientUserId { get; set; }

    public int ClientUserAuditUserId { get; set; }

    public DateTime ClientUserAuditDate { get; set; }

    [Required]
    public string ClientUserAuditBeforeJson { get; set; }

    [Required]
    public string ClientUserAuditAfterJson { get; set; }

    [Required]
    public string ClientUserAuditAffectedColumns { get; set; }

    [ForeignKey("ClientUserAuditActionId")]
    [InverseProperty("ClientUserAudits")]
    public virtual DataDictionary ClientUserAuditAction { get; set; }

    [ForeignKey("ClientUserAuditId")]
    [InverseProperty("ClientUserAudit")]
    public virtual ClientUser ClientUserAuditNavigation { get; set; }

    [ForeignKey("ClientUserAuditUserId")]
    [InverseProperty("ClientUserAudits")]
    public virtual User ClientUserAuditUser { get; set; }
}
