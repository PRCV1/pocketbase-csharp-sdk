using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;
using pocketbase_csharp_sdk.Extensions;
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
            var currentUserResult = await PocketBase.GetCurrentUserAsync();
            if (currentUserResult.IsSuccess)
            {
                _currentUser = currentUserResult.Value;
                var result = currentUserResult.Value;
                
                var avatarStreamResult = await PocketBase.Collection("users").DownloadFileAsync(_currentUser.Id, _currentUser.Avatar);
                
                if (avatarStreamResult.IsSuccess)
                {
                    _avatarAsBase64 = await GetBase64FromStream(avatarStreamResult.Value);
                }
            }
            
            await base.OnInitializedAsync();
        }

        private async Task<string> GetBase64FromStream(Stream stream)
        {
            await using MemoryStream ms = new();
            await stream.CopyToAsync(ms);

            var base64 = Convert.ToBase64String(ms.ToArray());
            return $"data:image/png;base64, {base64}";
        }

    }
}
