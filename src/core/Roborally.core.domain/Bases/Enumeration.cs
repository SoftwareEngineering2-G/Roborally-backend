namespace Roborally.core.domain.Bases;

public abstract class Enumeration
{
    public string DisplayName { get; }

/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 7" />
    protected Enumeration()
    {
        DisplayName = string.Empty;
    }

/// <author name="Truong Son NGO 2025-09-19 17:04:26 +0200 12" />
    protected Enumeration(string displayName)
    {
        DisplayName = displayName;
    }

/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 17" />
    public override string ToString()
    {
        return DisplayName;
    }

/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 22" />
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        bool typeMatches = GetType() == obj.GetType();
        bool displayNameMatches = DisplayName.Equals(otherValue.DisplayName);

        return typeMatches && displayNameMatches;
    }

/// <author name="Sachin Baral 2025-09-16 15:57:05 +0200 35" />
    public override int GetHashCode()
    {
        return DisplayName.GetHashCode();
    }
}