using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Portfolio.Components.Shared
{
    public partial class FriendlyLoadingSpinner : IDisposable
    {
        [Parameter]
        public string? Message { get; set; }
        [Parameter]
        public CancellationTokenSource? CancellationTokenSource { get; set; }

        protected Color LoadingColor
        {
            get
            {
                return ((int)LoadingPercentage / Breakpoint) switch
                {
                    0 => Color.Info, // < 40%
                    1 => Color.Warning, // < 80%
                    _ => Color.Error,
                };
            }
        }
        protected double LoadingPercentage { get; private set; } = 0;
        protected const int Breakpoint = 40;
        protected const int ImageHeight = 150;

        private const int _initialDelay = 8_000;
        private const int _interval = 1_000;
        private const double _increment = 100.0 / (Constants.Request.DefaultTimeoutSeconds - 5);

        private Timer? _timer = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _timer = new Timer(IncrementPercentage, null, _initialDelay, _interval);
        }

        private void IncrementPercentage(object? state)
        {
            if (LoadingPercentage == 0)
            {
                Message = "It looks like this might take a while... Time to call in the reinforcements";
                LoadingPercentage = 8;
            }
            else if (LoadingPercentage < 100)
            {
                LoadingPercentage += _increment;
            }
            else
            {
                _timer!.Dispose();
            }
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
