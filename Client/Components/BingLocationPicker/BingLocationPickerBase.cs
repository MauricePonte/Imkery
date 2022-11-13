using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Imkery.Client.Components
{
    public class BingLocationPickerBase : ComponentBase
    {
        [Inject]
        private IJSRuntime _JSRuntime { get; set; }

        private string _location;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _JSRuntime.InvokeAsync<Task>("loadMap");
            await base.OnAfterRenderAsync(firstRender);
        }

        public async void Test()
        {
            var result = await _JSRuntime.InvokeAsync<string>("getClickedLocationCoords");
            _location = Task.FromResult(result).Result;
            
        }
    }
}
