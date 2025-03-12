using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("DataDictionaryGroup")]
[Index("DataDictionaryGroupName", Name = "UQ_DataDictionaryGroupName", IsUnique = true)]
public partial class DataDictionaryGroup
{
    [Key]
    public int DataDictionaryGroupId { get; set; }

    [Required]
    [StringLength(50)]
    public string DataDictionaryGroupName { get; set; }

    public bool DataDictionaryGroupIsActive { get; set; }

    public bool DataDictionaryGroupIsDeleted { get; set; }

    [InverseProperty("DataDictionaryGroup")]
    public virtual ICollection<DataDictionary> DataDictionaries { get; set; } = new List<DataDictionary>();

    [InverseProperty("DataDictionaryGroupAuditDataDictionaryGroup")]
    public virtual ICollection<DataDictionaryGroupAudit> DataDictionaryGroupAudits { get; set; } = new List<DataDictionaryGroupAudit>();
}
