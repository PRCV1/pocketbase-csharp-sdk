using Example.DTOs;
using Example.Models;
using Mapster;
using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;

namespace Example.Pages.Components
{
    public partial class TodoListEntries
    {
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        private IEnumerable<EntryDTO> _entries;

        protected override async Task OnParametersSetAsync()
        {
            await LoadEntriesAsync();
            await base.OnParametersSetAsync();
        }

        protected async Task LoadEntriesAsync()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return;
            }

            var responseList = await PocketBase.Records.GetFullListAsync<EntryModel>("todos_entries");
            _entries = responseList.Adapt<IEnumerable<EntryDTO>>();
        }

    }
}
