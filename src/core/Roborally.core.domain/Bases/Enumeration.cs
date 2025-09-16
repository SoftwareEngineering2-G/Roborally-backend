namespace Roborally.core.domain.Bases;

public abstract class Enumeration 
{

    public string? Identifier { get; }
    public string DisplayName { get; }

    protected Enumeration()
    {
    }

    protected Enumeration(string identifier, string displayName)
    {
        Identifier = identifier;
        DisplayName = displayName;
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
        bool valueMatches = Identifier.Equals(otherValue.Identifier);
        bool displayNameMatches = DisplayName.Equals(otherValue.DisplayName);

        return typeMatches && valueMatches && displayNameMatches;
    }

    public override int GetHashCode()
    {
        return Identifier.GetHashCode() * DisplayName.GetHashCode();
    }

}