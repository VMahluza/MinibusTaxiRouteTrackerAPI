using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinibusTracker.Domain.Entities.Common;
public abstract class AuditableEntity 
{
    /// <summary>
    /// Base for tables that have created/updated timestamps.
    /// Columns match MySQL: created_at (UTC), updated_at (UTC, nullable).
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}