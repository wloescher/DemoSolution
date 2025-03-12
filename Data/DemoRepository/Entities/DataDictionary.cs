using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("DataDictionary")]
[Index("DataDictionaryKey", Name = "UQ_DataDictionaryKey", IsUnique = true)]
public partial class DataDictionary
{
    [Key]
    public int DataDictionaryId { get; set; }

    public int DataDictionaryGroupId { get; set; }

    [Required]
    [StringLength(50)]
    public string DataDictionaryKey { get; set; }

    public int DataDictionaryValue { get; set; }

    [StringLength(150)]
    public string DataDictionaryDescription { get; set; }

    public bool DataDictionaryIsActive { get; set; }

    public bool DataDictionaryIsDeleted { get; set; }

    [InverseProperty("ClientAuditAction")]
    public virtual ICollection<ClientAudit> ClientAudits { get; set; } = new List<ClientAudit>();

    [InverseProperty("ClientUserAuditAction")]
    public virtual ICollection<ClientUserAudit> ClientUserAudits { get; set; } = new List<ClientUserAudit>();

    [InverseProperty("ClientType")]
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    [InverseProperty("DataDictionaryAuditAction")]
    public virtual ICollection<DataDictionaryAudit> DataDictionaryAuditDataDictionaryAuditActions { get; set; } = new List<DataDictionaryAudit>();

    [InverseProperty("DataDictionaryAuditDataDictionary")]
    public virtual ICollection<DataDictionaryAudit> DataDictionaryAuditDataDictionaryAuditDataDictionaries { get; set; } = new List<DataDictionaryAudit>();

    [ForeignKey("DataDictionaryGroupId")]
    [InverseProperty("DataDictionaries")]
    public virtual DataDictionaryGroup DataDictionaryGroup { get; set; }

    [InverseProperty("DataDictionaryGroupAuditAction")]
    public virtual ICollection<DataDictionaryGroupAudit> DataDictionaryGroupAudits { get; set; } = new List<DataDictionaryGroupAudit>();

    [InverseProperty("UserAuditAction")]
    public virtual ICollection<UserAudit> UserAudits { get; set; } = new List<UserAudit>();

    [InverseProperty("UserType")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [InverseProperty("WorkItemAuditAction")]
    public virtual ICollection<WorkItemAudit> WorkItemAudits { get; set; } = new List<WorkItemAudit>();

    [InverseProperty("WorkItemStatus")]
    public virtual ICollection<WorkItem> WorkItemWorkItemStatuses { get; set; } = new List<WorkItem>();

    [InverseProperty("WorkItemType")]
    public virtual ICollection<WorkItem> WorkItemWorkItemTypes { get; set; } = new List<WorkItem>();
}
