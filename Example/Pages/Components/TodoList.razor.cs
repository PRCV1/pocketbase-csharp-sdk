using Example.Models;
using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;

namespace Example.Pages.Components
{
    public partial class TodoList
    {
        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        private bool isLoading = false;
        private IEnumerable<TodoModel>? todos;

        protected override async Task OnInitializedAsync()
        {
            await LoadTodosFromPocketbase();
            await base.OnInitializedAsync();
        }

        protected async Task LoadTodosFromPocketbase()
        {
            isLoading = true;
            todos = await PocketBase.Records.GetFullListAsync<TodoModel>("todos", batch: 1);
            isLoading = true;
        }

        protected void NavigateToTodoList(TodoModel model)
        {

        }

    }
}
