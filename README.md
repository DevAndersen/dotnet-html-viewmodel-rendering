# .NET HTML viewmodel rendering

A simple proof-of-concept of how to use the [`HtmlRenderer`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.htmlrenderer) class from [`Microsoft.AspNetCore.Components.Web`](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Web) and interface-bound viewmodels to render strongly typed Razor components into HTML strings.

Much nicer than storing email templates as large strings and then calling `.Replace` to swap out placeholder substrings with the actual content.

---

## Approach

- The project that contains the `.razor` files must use the razor SDK. This simply means editing the `.csproj` file so it begins with `<Project Sdk="Microsoft.NET.Sdk.Razor">`.This does not have to be the startup project, the views can be located in a separate project if desired.
- Define a razor view (`Email.razor`), a view model class (`EmailViewModel.cs`), and a view model interface (`IEmailViewModel`).
- Make the view model class implement the interface.
- Make the razor view implement the interface, using an `@implements` directive.
  - Members from the interface must be defined, either in a `@code` block.
    - Alternatively, declare them in a partial codebehind file (e.g. `Email.razor.cs`), which will often provide better diagnostics than using a `@code` block.
  - Members must be decorated with the `[Parameter]` interface, and optionally, the `[EditorRequired]` interface if desired.
    - The `required` keyword can be used to avoid potential null warnings.
- The logic in `HtmlRenderingService` created an `IDictionary<string, object?>` from the view model's properties, and populates them accordingly. It then uses the dictionary to create a `ParameterView`, which can be passed to the `HtmlRenderer`, which does the actual rendering.

## Example

### Razor view

```html
@implements IEmailViewModel

<p>Dear @RecipientName</p>

<p>A byte can represent any value from @byte.MinValue to @byte.MaxValue.</p>

<p>Kind regards, @SenderName</p>

<EmailFooter Date="DateTime.Now" />

@code
{
    [Parameter, EditorRequired]
    public required string RecipientName { get; init; }

    [Parameter, EditorRequired]
    public required string SenderName { get; init; }
}
```

### Code

```csharp
EmailViewModel viewModel = new EmailViewModel
{
    RecipientName = "John Doe",
    SenderName = "Jack Doe"
};

string html = await _htmlRenderingService.RenderComponentAsync<Email, IEmailViewModel>(viewModel);
Console.WriteLine(html);
```

### Output

```html
<p>Dear John Doe</p>

<p>A byte can represent any value from 0 to 255.</p>

<p>Kind regards, Jack Doe</p>

<div>Sent: 2026-06-09</div>
```
