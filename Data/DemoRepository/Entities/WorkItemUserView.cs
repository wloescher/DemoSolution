using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class WorkItemUserView
{
    public int WorkItemUserId { get; set; }

    public int WorkItemId { get; set; }

    public int UserId { get; set; }

    [StringLength(150)]
    public string WorkItemTitle { get; set; }

    [Required]
    [StringLength(100)]
    public string UserEmailAddress { get; set; }
}
