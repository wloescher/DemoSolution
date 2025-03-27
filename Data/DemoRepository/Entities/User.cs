using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("User")]
[Index("UserEmailAddress", Name = "UQ_UserEmailAddress", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    public Guid UserGuid { get; set; }

    public int UserTypeId { get; set; }

    [Required]
    [StringLength(100)]
    public string UserEmailAddress { get; set; }

    public bool UserIsActive { get; set; }

    public bool UserIsDeleted { get; set; }

    [StringLength(50)]
    public string UserPassword { get; set; }

    [StringLength(50)]
    public string UserFirstName { get; set; }

    [StringLength(50)]
    public string UserMiddleName { get; set; }

    [StringLength(50)]
    public string UserLastName { get; set; }

    [StringLength(255)]
    public string UserAddressLine1 { get; set; }

    [StringLength(255)]
    public string UserAddressLine2 { get; set; }

    [StringLength(50)]
    public string UserCity { get; set; }

    [StringLength(50)]
    public string UserRegion { get; set; }

    [StringLength(10)]
    public string UserPostalCode { get; set; }

    [StringLength(50)]
    public string UserCountry { get; set; }

    [StringLength(20)]
    public string UserPhoneNumber { get; set; }

    [StringLength(255)]
    public string UserPasswordHash { get; set; }

    public int UserPasswordAttemptCount { get; set; }

    [InverseProperty("ClientAuditUser")]
    public virtual ICollection<ClientAudit> ClientAudits { get; set; } = new List<ClientAudit>();

    [InverseProperty("ClientUserAuditUser")]
    public virtual ICollection<ClientUserAudit> ClientUserAudits { get; set; } = new List<ClientUserAudit>();

    [InverseProperty("ClientUserUser")]
    public virtual ICollection<ClientUser> ClientUsers { get; set; } = new List<ClientUser>();

    [InverseProperty("DataDictionaryAuditUser")]
    public virtual ICollection<DataDictionaryAudit> DataDictionaryAudits { get; set; } = new List<DataDictionaryAudit>();

    [InverseProperty("DataDictionaryGroupAuditUser")]
    public virtual ICollection<DataDictionaryGroupAudit> DataDictionaryGroupAudits { get; set; } = new List<DataDictionaryGroupAudit>();

    [InverseProperty("UserAuditUserIdSourceNavigation")]
    public virtual ICollection<UserAudit> UserAuditUserAuditUserIdSourceNavigations { get; set; } = new List<UserAudit>();

    [InverseProperty("UserAuditUser")]
    public virtual ICollection<UserAudit> UserAuditUserAuditUsers { get; set; } = new List<UserAudit>();

    [ForeignKey("UserTypeId")]
    [InverseProperty("Users")]
    public virtual DataDictionary UserType { get; set; }

    [InverseProperty("WorkItemAuditUser")]
    public virtual ICollection<WorkItemAudit> WorkItemAudits { get; set; } = new List<WorkItemAudit>();

    [InverseProperty("WorkItemUserAuditUser")]
    public virtual ICollection<WorkItemUserAudit> WorkItemUserAudits { get; set; } = new List<WorkItemUserAudit>();

    [InverseProperty("WorkItemUserUser")]
    public virtual ICollection<WorkItemUser> WorkItemUsers { get; set; } = new List<WorkItemUser>();
}
