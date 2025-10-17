using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinibusTracker.Application.DTO.Associations;

public abstract class BaseAssociation
{
       
        public string Name { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? ContactPhone { get; set; }
       
}

