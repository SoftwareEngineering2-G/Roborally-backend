namespace Roborally.core.domain.Bases;

public abstract class Enumeration 
{
    public string DisplayName { get; }

    protected Enumeration()
    {
        DisplayName = string.Empty;
    }

    protected Enumeration(string displayName) {
        DisplayName = displayName;
    }
    
    public override string ToString()
    {
        return DisplayName;
    }

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

    public override int GetHashCode()
    {
        return DisplayName.GetHashCode();
    }

}