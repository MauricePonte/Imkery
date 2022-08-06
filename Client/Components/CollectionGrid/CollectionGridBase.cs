using Imkery.API.Client;
using Imkery.API.Client.Core;
using Imkery.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Imkery.Client.Components
{
    public class CollectionGridBase<TItem> : ComponentBase where TItem : class, IEntity<TItem>, new()
    {
        [Inject]
        public ApiClientRegistry ApiClientRegistry { get; set; }
        [Inject]
        private IDialogService DialogService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public List<CollectionField> Fields { get; set; } = new List<CollectionField>();

        protected MudTable<TItem> _table;

        [Parameter]
        public bool ShowDeleteButton { get; set; }
        [Parameter]
        public bool ShowEditButton { get; set; }
        [Parameter]
        public bool ShowAddButton { get; set; }
        [Parameter]
        public string EditPage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ApiClient = ApiClientRegistry.GetApiClientForTypeAs<BaseCRUDClient<TItem>>(typeof(TItem));
        }

        protected async Task<TableData<TItem>> GetCurrentCollectionAsync(TableState state)
        {
            var filterOptions = new FilterPagingOptions()
            {
                SortProperty = state.SortLabel + ";" + (state.SortDirection == SortDirection.Descending ? "Down" : "Up"),
                FilterParameters = new Dictionary<string, string>() {
                    { "searchText", SearchText} },
                ItemsPerPage = state.PageSize,
                Page = state.Page
            };
            var data = await ApiClient.GetCollectionAsync(filterOptions);
            var totalCount = await ApiClient.GetCountAsync(filterOptions);
            var tableSource = new TableData<TItem>();
            tableSource.Items = data;
            tableSource.TotalItems = totalCount;
            return tableSource;
        }

        private TItem _currentItem;
        public TItem CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                _currentItem = value;
                CurrentObjectChanged?.Invoke(value);
            }
        }

        public Action<TItem> CurrentObjectChanged { get; set; } = new Action<TItem>((item) => { });
        public string SearchText { get; set; } = String.Empty;
        public BaseCRUDClient<TItem> ApiClient { get; private set; }

        protected void DoSearch(string text)
        {
            SearchText = text;
            _table.ReloadServerData();
        }

        public string? GetDisplayValue(TItem currentDataItem, CollectionField field)
        {
            var typeOfItem = typeof(TItem);
            var property = typeOfItem.GetProperty(field.PropertyPath);
            return property?.GetValue(currentDataItem)?
                .ToString();
        }

        protected List<TItem> ChangingItems { get; set; } = new List<TItem>();
        public async Task<bool> DeleteItem(TItem item)
        {
            bool? result = await DialogService.ShowMessageBox(
             "Delete",
             $"Are you sure you want to delete '{item.GetDescription()}'?",
             yesText: "Yes", cancelText: "No");
            if (result.HasValue && result.Value)
            {
                ChangingItems.Add(item);
                await ApiClient.DeleteAsync(item.Id);
                await _table.ReloadServerData();
                ChangingItems.Remove(item);
                return true;
            }
            return false;
        }
        public async Task<bool> EditItem(TItem item)
        {
            NavigationManager.NavigateTo(EditPage + "/" + item.Id.ToString());
            return true;
        }
        public async Task<bool> AddItem()
        {
            NavigationManager.NavigateTo(EditPage);
            return true;
        }
        


    }
}