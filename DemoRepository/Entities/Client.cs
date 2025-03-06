using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("Client")]
[Index("ClientName", Name = "UQ_ClientName", IsUnique = true)]
public partial class Client
{
    [Key]
    public int ClientId { get; set; }

    public Guid ClienttGuid { get; set; }

    public int ClientTypeId { get; set; }

    [Required]
    [StringLength(100)]
    public string ClientName { get; set; }

    public bool ClientIsActive { get; set; }

    public bool ClientIsDeleted { get; set; }

    [StringLength(255)]
    public string ClientAddress { get; set; }

    [StringLength(50)]
    public string ClientCity { get; set; }

    [StringLength(50)]
    public string ClientRegion { get; set; }

    [StringLength(10)]
    public string ClientPostalCode { get; set; }

    [StringLength(50)]
    public string ClientCountry { get; set; }

    [StringLength(150)]
    public string ClientUrl { get; set; }

    [InverseProperty("ClientAuditClient")]
    public virtual ICollection<ClientAudit> ClientAudits { get; set; } = new List<ClientAudit>();

    [ForeignKey("ClientTypeId")]
    [InverseProperty("Clients")]
    public virtual DataDictionary ClientType { get; set; }

    [InverseProperty("ClientUserClient")]
    public virtual ICollection<ClientUser> ClientUsers { get; set; } = new List<ClientUser>();

    [InverseProperty("WorkItemClient")]
    public virtual ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}
