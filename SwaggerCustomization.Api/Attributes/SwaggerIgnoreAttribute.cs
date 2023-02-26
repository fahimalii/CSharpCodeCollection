namespace SwaggerCustomization.Api.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class SwaggerIgnoreAttribute : Attribute
{

}
