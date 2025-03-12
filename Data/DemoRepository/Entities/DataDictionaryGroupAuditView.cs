using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class DataDictionaryGroupAuditView
{
    public int DataDictionaryGroupAuditId { get; set; }

    public int ActionId { get; set; }

    [StringLength(50)]
    public string Action { get; set; }

    public int DataDictionaryGroupId { get; set; }

    public int UserId { get; set; }

    [StringLength(50)]
    public string DataDictionaryGroupName { get; set; }

    [StringLength(101)]
    public string UserFullName { get; set; }

    [StringLength(100)]
    public string UserEmailAddress { get; set; }

    public DateTime Date { get; set; }

    [Required]
    public string BeforeJson { get; set; }

    [Required]
    public string AfterJson { get; set; }

    [Required]
    public string AffectedColumns { get; set; }
}
