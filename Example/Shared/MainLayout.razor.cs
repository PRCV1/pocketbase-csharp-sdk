using MudBlazor;

namespace Example.Shared
{
    public partial class MainLayout
    {

        public bool UseDarkmode { get; set; } = false;

        //fields
        protected MudThemeProvider? themeProvider = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                UseDarkmode = await themeProvider!.GetSystemPreference();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

    }
}
