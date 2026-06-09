using Microsoft.Extensions.Hosting;

namespace ConsoleApp;

public class HostedService : IHostedService
{
    private readonly IHost _host;
    private readonly HtmlRenderingService _htmlRenderingService;

    public HostedService(IHost host, HtmlRenderingService htmlRenderingService)
    {
        _host = host;
        _htmlRenderingService = htmlRenderingService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        EmailViewModel viewModel = new EmailViewModel
        {
            RecipientName = "John Doe",
            SenderName = "Jack Doe"
        };

        string html = await _htmlRenderingService.RenderComponentAsync<Email, IEmailViewModel>(viewModel);
        Console.WriteLine(html);

        await _host.StopAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
