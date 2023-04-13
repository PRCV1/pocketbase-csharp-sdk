using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;
using pocketbase_csharp_sdk.Models;

namespace Example.Pages.Login.Components
{
    public partial class Profile
    {

        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        protected UserModel? _currentUser = null;
        protected string? _avatarAsBase64 = null;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await PocketBase.User.GetOneAsync(PocketBase.AuthStore.Model.Id);
            var avatarStream = await PocketBase.Records.DownloadFileAsync("users", PocketBase.AuthStore.Model.Id, _currentUser.Avatar);

            _avatarAsBase64 = await GetBase64FromStream(avatarStream);

            await base.OnInitializedAsync();
        }

        private async Task<string> GetBase64FromStream(Stream stream)
        {
            if (stream is null)
            {
                return string.Empty;
            }

            await using MemoryStream ms = new();
            await stream.CopyToAsync(ms);

            var base64 = Convert.ToBase64String(ms.ToArray());
            return $"data:image/png;base64, {base64}";
        }

    }
}
