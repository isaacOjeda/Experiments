using BlazorHtmxWeb.Common.Extensions;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlazorHtmxWeb.Common.Handlers;


public static class PageResults
{
    public static IResult Page<TComponent>(object data)
    {
        var componentData = data.ToDictionary();        
        
        return new RazorComponentResult(typeof(TComponent), componentData);
    }
}