using AutoMapper.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StayCloudAPI.WebAPI.Filters
{
    public class SwaggerNullableParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (!parameter.Schema.Nullable &&
                (context.ApiParameterDescription.Type.IsNullableType()))
            {
                parameter.Schema.Nullable = true;
            }
        }
    }
}
