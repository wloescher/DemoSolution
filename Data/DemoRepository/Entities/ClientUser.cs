﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Table("ClientUser")]
[Index("ClientUserClientId", "ClientUserUserId", Name = "UQ_ClientUser", IsUnique = true)]
public partial class ClientUser
{
    [Key]
    public int ClientUserId { get; set; }

    public int ClientUserClientId { get; set; }

    public int ClientUserUserId { get; set; }

    public bool ClientUserIsDeleted { get; set; }

    [InverseProperty("ClientUserAuditClientUser")]
    public virtual ICollection<ClientUserAudit> ClientUserAudits { get; set; } = new List<ClientUserAudit>();

    [ForeignKey("ClientUserClientId")]
    [InverseProperty("ClientUsers")]
    public virtual Client ClientUserClient { get; set; }

    [ForeignKey("ClientUserUserId")]
    [InverseProperty("ClientUsers")]
    public virtual User ClientUserUser { get; set; }
}
