using MinibusTracker.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinibusTracker.Domain.Entities;
[Table("associations")]
public class Association : AuditableEntity
{
    /// <summary>
    /// Represents a taxi association or operator. | One-to-Many → Routes 
    /// </summary>
    [Column("association_id")]
    public Guid AssociationId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("region")]
    public string? Region { get; set; }

    [Column("contact_phone")]
    public string? ContactPhone { get; set; }
}