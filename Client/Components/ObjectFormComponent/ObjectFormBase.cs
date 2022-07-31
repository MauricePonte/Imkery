using FluentValidation;
using Imkery.API.Client;
using Imkery.API.Client.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Imkery.Client.Components.ObjectFormComponent
{
    public class ObjectFormBase<TItem> : ComponentBase where TItem : class, IEntity<TItem>, new()
    {
        private bool _isValid = false;
        [Parameter]
        public Guid ItemId { get; set; }

        [Parameter]
        public TItem EditingItem { get; set; }


        [Parameter]
        public Func<TItem, string> ReturnUrl { get; set; }

        public BaseCRUDClient<TItem> ApiClient { get; private set; }

        [Inject]
        private ApiClientRegistry ApiClientRegistry { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public RenderFragment<TItem> FormContent { get; set; }

        [Parameter]
        public string Title { get; set; }

        protected MudForm form { get; set; }
        public string[] Errors { get; set; }
        public bool Success { get; set; }
        public bool IsSaving { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ApiClient = ApiClientRegistry.GetApiClientForTypeAs<BaseCRUDClient<TItem>>(typeof(TItem));
            await DetermineEditObject();
        }

        private async Task DetermineEditObject()
        {
            if (EditingItem == null)
            {
                if (ItemId != Guid.Empty)
                {
                    EditingItem = await ApiClient.GetItemAsync(ItemId);
                }
                else
                {
                    EditingItem = new TItem();
                    StateHasChanged();
                }
            }
        }

        public async Task<bool> SubmitItem()
        {
            await DoValidate();
            if (!IsSaving && Success && _isValid)
            {
                IsSaving = true;
                if (ItemId != Guid.Empty)
                {
                    await ApiClient.EditAsync(ItemId, EditingItem);
                }
                else
                {
                    await ApiClient.AddAsync(EditingItem);
                }
                NavigationManager.NavigateTo(ReturnUrl.Invoke(EditingItem));
                return true;
            }

            return false;
        }

        public async Task DoValidate()
        {
            var validator = EditingItem.GetValidator();
            var result = await validator.ValidateAsync(ValidationContext<TItem>.CreateWithOptions(EditingItem, options => options.IncludeAllRuleSets()));
            if (result.IsValid)
            {
                _isValid = true;
                return;
            }
            _isValid = false;
            Errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
        }
    }
}
