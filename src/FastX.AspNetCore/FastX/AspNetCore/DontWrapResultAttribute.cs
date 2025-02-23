namespace FastX.AspNetCore;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class DontWrapResultAttribute : Attribute
{
}