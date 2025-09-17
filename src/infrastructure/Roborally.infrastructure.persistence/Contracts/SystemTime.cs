using Roborally.core.domain.Bases;

namespace Roborally.infrastructure.persistence.Contracts;

public class SystemTime : ISystemTime
{
    public DateTime CurrentTime => DateTime.UtcNow;
}