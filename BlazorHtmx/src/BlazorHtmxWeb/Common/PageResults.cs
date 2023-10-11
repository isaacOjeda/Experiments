using BlazorHtmxWeb.Common.Extensions;
using Microsoft.AspNetCore.Components.Endpoints;

namespace BlazorHtmxWeb.Common.Handlers;


public static class PageResults
{
    public static IResult Page<TComponent>(object data)
    {
        var componentData = data.ToDictionary();        
        
        return new RazorComponentResult(typeof(TComponent), componentData);
    }
}