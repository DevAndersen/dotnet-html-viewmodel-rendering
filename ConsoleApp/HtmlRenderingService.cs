using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using System.Collections.Immutable;
using System.Reflection;

namespace ConsoleApp;

public class HtmlRenderingService
{
    private readonly HtmlRenderer _htmlRenderer;

    public HtmlRenderingService(HtmlRenderer htmlRenderer)
    {
        _htmlRenderer = htmlRenderer;
    }

    public async Task<string> RenderComponentAsync<TComponent, TViewModel>(TViewModel viewModel)
        where TComponent : IComponent, TViewModel
    {
        IDictionary<string, object?> dictionary = CreatePropertyDictionary(viewModel);
        ParameterView parameters = ParameterView.FromDictionary(dictionary);

        return await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            HtmlRootComponent component = await _htmlRenderer.RenderComponentAsync<TComponent>(parameters);
            return component.ToHtmlString();
        });
    }

    private static IDictionary<string, object?> CreatePropertyDictionary<T>(T? model)
    {
        if (model == null)
        {
            return ImmutableDictionary<string, object?>.Empty;
        }

        PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.ToDictionary(k => k.Name, v => v.GetValue(model));
    }
}
