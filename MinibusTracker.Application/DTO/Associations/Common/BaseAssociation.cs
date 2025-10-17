namespace MinibusTracker.Application.DTO.Associations.Common;

public abstract class BaseAssociation
{
        public string Name { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? ContactPhone { get; set; }    
}

