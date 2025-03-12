using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class ClientView
{
    public int ClientId { get; set; }

    public Guid Guid { get; set; }

    public int TypeId { get; set; }

    [StringLength(50)]
    public string Type { get; set; }

    public bool IsActive { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(255)]
    public string AddressLine1 { get; set; }

    [StringLength(255)]
    public string AddressLine2 { get; set; }

    [StringLength(50)]
    public string City { get; set; }

    [StringLength(50)]
    public string Region { get; set; }

    [StringLength(10)]
    public string PostalCode { get; set; }

    [StringLength(50)]
    public string Country { get; set; }

    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [StringLength(150)]
    public string Url { get; set; }
}
