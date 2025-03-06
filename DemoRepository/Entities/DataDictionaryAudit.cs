﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("DataDictionaryAudit")]
public partial class DataDictionaryAudit
{
    [Key]
    public int DataDictionaryAuditId { get; set; }

    public int DataDictionaryAuditActionId { get; set; }

    public int DataDictionaryAuditDataDictionaryId { get; set; }

    public int DataDictionaryAuditUserId { get; set; }

    public DateTime DataDictionaryAuditDate { get; set; }

    [Required]
    public string DataDictionaryAuditBeforeJson { get; set; }

    [Required]
    public string DataDictionaryAuditAfterJson { get; set; }

    [Required]
    public string DataDictionaryAuditAffectedColumns { get; set; }

    [ForeignKey("DataDictionaryAuditActionId")]
    [InverseProperty("DataDictionaryAuditDataDictionaryAuditActions")]
    public virtual DataDictionary DataDictionaryAuditAction { get; set; }

    [ForeignKey("DataDictionaryAuditDataDictionaryId")]
    [InverseProperty("DataDictionaryAuditDataDictionaryAuditDataDictionaries")]
    public virtual DataDictionary DataDictionaryAuditDataDictionary { get; set; }

    [ForeignKey("DataDictionaryAuditUserId")]
    [InverseProperty("DataDictionaryAudits")]
    public virtual User DataDictionaryAuditUser { get; set; }
}
