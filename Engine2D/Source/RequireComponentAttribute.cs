namespace Engine2D;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RequireComponentAttribute<T> : Attribute where T : Component
{ }
