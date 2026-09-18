using Microsoft.AspNetCore.Components;
using WarehouseManagement.Services;

namespace WarehouseManagement.Components.Pages;

public partial class IndexMk
{
    protected bool MenuOpen { get; set; }

    protected bool Submitted { get; set; }
    protected bool Sending { get; set; }

    protected string? EmailError { get; set; }

    [Inject]
    protected EmailService EmailService { get; set; } = default!;

    protected EmailService.DemoRequest DemoRequestItem { get; set; } = new();

    protected void ToggleMenu()
    {
        MenuOpen = !MenuOpen;
    }

    protected void CloseMenu()
    {
        MenuOpen = false;
    }
    protected void InvalidForm()
    {
        EmailError = "Bitte füllen Sie alle Pflichtfelder korrekt aus.";
    }
    protected async Task SubmitDemo()
    {
        EmailError = null;
        Submitted = false;
        Sending = true;

        try
        {
            await EmailService.SendDemoRequest(DemoRequestItem);

            Submitted = true;

            // Formularot se prazni po uspešno isprakjanje
            DemoRequestItem = new EmailService.DemoRequest();
        }
        catch (Exception ex)
        {
            EmailError =
                "Die Anfrage konnte nicht gesendet werden. " +
                "Bitte versuchen Sie es erneut.";

            Console.WriteLine("========================================");
            Console.WriteLine("FEHLER BEIM VERSENDEN DER DEMO-EMAIL");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("========================================");
        }
        finally
        {
            Sending = false;
        }
    }
}