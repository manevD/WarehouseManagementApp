using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Components.Pages;

public partial class Index
{
    protected bool MenuOpen { get; set; }

    protected bool Submitted { get; set; }

    protected DemoRequest DemoRequestItem { get; set; } = new();

    protected void ToggleMenu()
    {
        MenuOpen = !MenuOpen;
    }

    protected void CloseMenu()
    {
        MenuOpen = false;
    }

    protected void SubmitDemo()
    {
        Submitted = true;
    }

    public sealed class DemoRequest
    {
        [Required(ErrorMessage = "Bitte Namen eingeben.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte Unternehmen eingeben.")]
        public string Company { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte E-Mail eingeben.")]
        [EmailAddress(ErrorMessage = "Bitte gültige E-Mail eingeben.")]
        public string Email { get; set; } = string.Empty;

        public string Industry { get; set; } = "Großhandel";

        public string Message { get; set; } = string.Empty;
    }
}
