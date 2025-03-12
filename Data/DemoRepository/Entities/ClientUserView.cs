using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

[Keyless]
public partial class ClientUserView
{
    public int ClientUserId { get; set; }

    public int ClientId { get; set; }

    public int UserId { get; set; }

    [StringLength(100)]
    public string ClientName { get; set; }

    [StringLength(101)]
    public string UserFullName { get; set; }

    [StringLength(100)]
    public string UserEmailAddress { get; set; }
}
