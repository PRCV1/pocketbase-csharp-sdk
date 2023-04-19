using Example.Models;
using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;
using static MudBlazor.CategoryTypes;

namespace Example.Pages.SharedComponents
{
    public partial class TodoListEntries
    {
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        private List<EntryModel>? _entries;

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

            var result =
                await PocketBase.Records.GetFullListAsync<EntryModel>("todos_entries", filter: $"todo_id.id='{Id}'");
            if (result.IsSuccess)
            {
                _entries = result.Value.ToList();
            }
        }

        protected void AddNewEntry()
        {
            if (_entries is null)
            {
                return;
            }
            _entries.Add(new EntryModel());
        }

        protected void Remove(EntryModel item)
        {
            if (_entries is null)
            {
                return;
            }
            _entries.Remove(item);
        }

        protected async Task SaveAsync()
        {
            if (_entries is null)
            {
                return;
            }
            foreach (var item in _entries)
            {
                await PocketBase.Records.UpdateAsync<EntryModel>("todos_entries", item);
            }
        }

    }
}
