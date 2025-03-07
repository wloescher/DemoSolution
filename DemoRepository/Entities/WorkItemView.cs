using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class WorkItemView
{
    public int WorkItemId { get; set; }

    public Guid Guid { get; set; }

    public int ClientId { get; set; }

    [StringLength(100)]
    public string ClientName { get; set; }

    public int TypeId { get; set; }

    [StringLength(50)]
    public string Type { get; set; }

    public int StatusId { get; set; }

    [StringLength(50)]
    public string Status { get; set; }

    public bool IsActive { get; set; }

    [StringLength(150)]
    public string Title { get; set; }

    [StringLength(150)]
    public string SubTitle { get; set; }

    [StringLength(500)]
    public string Summary { get; set; }

    public string Body { get; set; }
}
