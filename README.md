# dotnet-html-viewmodel-rendering
Proof-of-concept for viewmodel-bound rendering of razor components into an HTML string.

---

- The project that contains the `.razor` files must use the razor SDK. This simply means editing the `.csproj` file so it begins with `<Project Sdk="Microsoft.NET.Sdk.Razor">`.This does not have to be the startup project, the views can be located in a separate project if desired.
- Define a razor view (`Email.razor`), a view model class (`EmailViewModel.cs`), and a view model interface (`IEmailViewModel`).
- Make the view model class implement the interface.
- Make the razor view implement the interface, using an `@implements` directive.
  - Members from the interface must be defined, either in a `@code` block.
    - Alternatively, declare them in a partial codebehind file (e.g. `Email.razor.cs`), which will often provide better diagnostics than using a `@code` block.
  - Members must be decorated with the `[Parameter]` interface, and optionally, the `[EditorRequired]` interface if desired.
    - The `required` keyword can be used to avoid potential null warnings.
- The logic in `HtmlRenderingService` created an `IDictionary<string, object?>` from the view model's properties, and populates them accordingly. It then uses the dictionary to create a `ParameterView`, which can be passed to the `HtmlRenderer`, which does the actual rendering.
