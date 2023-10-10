using Example.Models;
using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;

namespace Example.Pages.SharedComponents
{
    public partial class TodoList
    {
        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        private bool _loading = false;
        private IEnumerable<TodoModel>? todos;

        protected override async Task OnInitializedAsync()
        {
            await LoadTodosFromPocketbase();
            await base.OnInitializedAsync();
        }

        private async Task LoadTodosFromPocketbase()
        {
            _loading = true;
            
            var result = await PocketBase.Collection("todos").GetFullListAsync<TodoModel>();
            
            
            if (result.IsSuccess)
            {
                todos = result.Value;
            }
            
            _loading = false;
        }

        private void GoToDetails(TodoModel dto)
        {
            var path = $"/details/{dto.Id}";
            NavigationManager.NavigateTo(path);
        }

    }
}
