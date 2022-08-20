using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Imkery.Client.Components
{
    public class BingLocationPickerBase : ComponentBase
    {
        [Inject]
        private IJSRuntime _JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _JSRuntime.InvokeAsync<Task>("loadMap");
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
