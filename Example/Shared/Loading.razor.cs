using Microsoft.AspNetCore.Components;

namespace Example.Shared
{
    public partial class Loading
    {
        [Parameter]
        public bool? Visible { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

    }
}
