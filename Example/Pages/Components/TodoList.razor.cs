using Example.DTOs;
using Example.Models;
using Mapster;
using Microsoft.AspNetCore.Components;
using pocketbase_csharp_sdk;

namespace Example.Pages.Components
{
    public partial class TodoList
    {
        [Inject]
        public PocketBase PocketBase { get; set; } = null!;

        private bool _loading = false;
        private IEnumerable<TodoDTO>? todos;

        protected override async Task OnInitializedAsync()
        {
            await LoadTodosFromPocketbase();
            await base.OnInitializedAsync();
        }

        private async Task LoadTodosFromPocketbase()
        {
            _loading = true;
            var responseList = await PocketBase.Records.GetFullListAsync<TodoModel>("todos");
            todos = responseList.Adapt<IEnumerable<TodoDTO>>();
            _loading = false;
        }

        private void ToggleDetails(TodoDTO dto)
        {
            dto.ShowDetails = !dto.ShowDetails;
        }

    }
}
