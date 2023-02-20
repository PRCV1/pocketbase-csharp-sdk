using Example.Models;
using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;

namespace Example.Pages.Components
{
    public partial class TodoList
    {
        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadTodosFromPocketbase();
            await base.OnInitializedAsync();
        }

        protected async Task LoadTodosFromPocketbase()
        {
            var list = await PocketBase.Records.ListAsync<TodoModel>("todos");
        }

    }
}
