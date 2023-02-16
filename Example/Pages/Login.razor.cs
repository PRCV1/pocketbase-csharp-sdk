using Microsoft.AspNetCore.Components;
using MudBlazor;
using pocketbase_csharp_sdk;

namespace Example.Pages
{
    public partial class Login
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        public string? Username { get; set; }
        public string? Password { get; set; }

        protected async Task LoginAsync()
        {
            var valid = await CheckInputs();
            if (valid)
            {
                try
                {
                    var lel = await PocketBase.User.AuthenticateWithPasswordAsync(Username!, Password!);
                    if (PocketBase.AuthStore.IsValid)
                    {
                        Snackbar.Add("Logged in!", Severity.Success);
                        NavigationManager.NavigateTo("/");
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add("Login failed, please check your Username and Password", Severity.Error);
                }
                
            }
        }

        private async Task<bool> CheckInputs()
        {
            var userEmpty = string.IsNullOrWhiteSpace(Username);
            var passwordEmpty = string.IsNullOrWhiteSpace(Password);

            if (userEmpty || passwordEmpty)
            {
                Snackbar.Add("The Username und Password fields are required.", Severity.Warning);
                return false;
            }
            return true;
        }

    }
}
