using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class WorkItemAuditView
{
    public int WorkItemAuditId { get; set; }

    public int ActionId { get; set; }

    [Required]
    [StringLength(50)]
    public string Action { get; set; }

    public int WorkItemId { get; set; }

    public int UserId { get; set; }

    [StringLength(150)]
    public string WorkItemTitle { get; set; }

    [Required]
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
