using MainApp.Components.Spinner;
using MainApp.Components.Toast;
using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.SetupPage.Bank
{
    public partial class SetupBankLeftPanel : ComponentBase
    {
        [Inject]
        IBankService<BankModel> _bankService { get; set; } = default!;

        [Inject]
        private ToastService _toastService { get; set; } = new();

        [Inject]
        private SpinnerService _spinnerService { get; set; } = new();

        /*
         * Add OffCanvas component reference
         */
        private SetupBankOffCanvas _setupOffCanvas { get; set; } = new();
        private BankModel _bankModel { get; set; } = new();

        private List<BankModel> _banks { get; set; } = new();
        private List<BankModel> _searchResults { get; set; } = new();
        private string _searchTerm { get; set; } = string.Empty;
        private bool _isSearching { get; set; } = false;
        private bool _isLoading { get; set; } = true;
        private bool _searchButtonEnabled { get; set; } = false;

        public SetupBankLeftPanel()
        {
        }

        protected async override Task OnInitializedAsync()
        {
            await FetchDataAsync();
            await Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Run(() => _spinnerService.ShowSpinner());
            }

            await Task.CompletedTask;
        }

        private async Task SearchTermAsync(ChangeEventArgs eventArgs)
        {
            var searchTerm = eventArgs?.Value?.ToString();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _searchResults = new();
                _isSearching = false;
                _searchButtonEnabled = false;
            }
            else
            {
                _searchButtonEnabled = true;
            }
            await Task.CompletedTask;
        }

        private async Task SearchAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_searchTerm))
                {
                    _isSearching = true;
                    _searchResults = await _bankService.GetSearchResults(_searchTerm);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast(ex.Message, Theme.Danger);

            }

            await Task.CompletedTask;
        }

        private async Task FetchDataAsync()
        {
            try
            {
                _banks = await _bankService.GetRecords();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }

            await Task.CompletedTask;
        }

        private async Task UpdateStatusAsync(BankModel bankModel)
        {
            try
            {
                await _bankService.UpdateRecordStatus(bankModel);
            }
            catch (Exception ex)
            {
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
            await Task.CompletedTask;
        }

        private async Task RefreshList()
        {
            // TODO: add service to refresh the list
            BankModel updatedModel = _setupOffCanvas.DataModel;
            BankModel model = _banks.FirstOrDefault(b => b.Id == updatedModel.Id)!;

            var index = _banks.IndexOf(model);

            if (index != -1)
            {
                if (updatedModel.IsArchived)
                {
                    var archivedModel = _banks[index] = updatedModel;
                    _banks.Remove(archivedModel);
                }
                else
                {
                    _banks[index] = updatedModel;
                }
            }
            else
            {
                _banks.Add(updatedModel);
            }

            await InvokeAsync(StateHasChanged);
            await Task.CompletedTask;
        }

        private async Task AddRecordAsync()
        {
            await _setupOffCanvas.AddRecordOffCanvasAsync();
            await Task.CompletedTask;
        }

        private async Task EditRecordAsync(BankModel bankModel)
        {
            try
            {
                await _setupOffCanvas.EditRecordOffCanvasAsync(bankModel.Id.ToString());
            }
            catch (Exception ex)
            {
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
            await Task.CompletedTask;
        }

        private async Task ViewRecordAsync(BankModel bankModel)
        {
            try
            {
                await _setupOffCanvas.ViewRecordOffCanvasAsync(bankModel.Id.ToString());
            }
            catch (Exception ex)
            {
                await Task.Delay((int)Delay.DataError);
                _toastService.ShowToast(ex.Message, Theme.Danger);
            }
            await Task.CompletedTask;
        }
    }
}
