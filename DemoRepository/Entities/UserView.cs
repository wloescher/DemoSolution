using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class UserView
{
    public int UserId { get; set; }

    public Guid Guid { get; set; }

    public int TypeId { get; set; }

    [StringLength(50)]
    public string Type { get; set; }

    public bool IsActive { get; set; }

    [Required]
    [StringLength(100)]
    public string EmailAddress { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string MiddleName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(153)]
    public string FullName { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    [StringLength(50)]
    public string City { get; set; }

    [StringLength(50)]
    public string Region { get; set; }

    [StringLength(10)]
    public string PostalCode { get; set; }

    [StringLength(50)]
    public string Country { get; set; }
}
