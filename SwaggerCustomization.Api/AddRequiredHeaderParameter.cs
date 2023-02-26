using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerCustomization.Api;

public sealed class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        // If we need to apply this conditionally
        var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

        if (descriptor != null && descriptor.ControllerName.Equals("WeatherForecast", StringComparison.OrdinalIgnoreCase) && descriptor.ActionName.Equals("Test", StringComparison.OrdinalIgnoreCase))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Name",
                In = ParameterLocation.Header,
                Required = true
            });
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-apisubscription-key",
            In = ParameterLocation.Header,
            Required = true
        });


    }
}
