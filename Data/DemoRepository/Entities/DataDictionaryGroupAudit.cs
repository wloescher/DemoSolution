﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("DataDictionaryGroupAudit")]
public partial class DataDictionaryGroupAudit
{
    [Key]
    public int DataDictionaryGroupAuditId { get; set; }

    public int DataDictionaryGroupAuditActionId { get; set; }

    public int DataDictionaryGroupAuditDataDictionaryGroupId { get; set; }

    public int DataDictionaryGroupAuditUserId { get; set; }

    public DateTime DataDictionaryGroupAuditDate { get; set; }

    [Required]
    public string DataDictionaryGroupAuditBeforeJson { get; set; }

    [Required]
    public string DataDictionaryGroupAuditAfterJson { get; set; }

    [Required]
    public string DataDictionaryGroupAuditAffectedColumns { get; set; }

    [ForeignKey("DataDictionaryGroupAuditActionId")]
    [InverseProperty("DataDictionaryGroupAudits")]
    public virtual DataDictionary DataDictionaryGroupAuditAction { get; set; }

    [ForeignKey("DataDictionaryGroupAuditDataDictionaryGroupId")]
    [InverseProperty("DataDictionaryGroupAudits")]
    public virtual DataDictionaryGroup DataDictionaryGroupAuditDataDictionaryGroup { get; set; }

    [ForeignKey("DataDictionaryGroupAuditUserId")]
    [InverseProperty("DataDictionaryGroupAudits")]
    public virtual User DataDictionaryGroupAuditUser { get; set; }
}
