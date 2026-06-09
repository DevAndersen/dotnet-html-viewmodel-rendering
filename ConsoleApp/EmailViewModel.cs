namespace ConsoleApp;

public class EmailViewModel : IEmailViewModel
{
    public required string RecipientName { get; init; }

    public required string SenderName { get; init; }
}
