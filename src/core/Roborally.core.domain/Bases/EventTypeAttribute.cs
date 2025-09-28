namespace Roborally.core.domain.Bases;

[AttributeUsage(AttributeTargets.Class)]
public class EventTypeAttribute : Attribute
{
    public string Name { get; }
    
    public EventTypeAttribute(string name)
    {
        Name = name;
    }
}
