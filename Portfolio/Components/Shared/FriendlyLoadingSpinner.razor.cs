using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Portfolio.Components.Shared
{
    public partial class FriendlyLoadingSpinner : IDisposable
    {
        [Parameter, EditorRequired]
        public required int StartAnimationAfterXSec { get; set; } = 1;
        [Parameter, EditorRequired]
        public required int EndAnimationAfterXSec { get; set; } = 1;

        [Parameter]
        public string? Message { get; set; }
        [Parameter]
        public CancellationTokenSource? CancellationTokenSource { get; set; }

        protected Color LoadingColor
        {
            get
            {
                return ((int)LoadingPercentage / BreakpointProc) switch
                {
                    0 => Color.Info, // < 40%
                    1 => Color.Warning, // < 80%
                    _ => Color.Error,
                };
            }
        }
        protected double LoadingPercentage { get; private set; } = 0;
        protected const int BreakpointProc = 40;
        protected const int ImageHeight = 150;

        private double Increment => 100.0 / EndAnimationAfterXSec;

        private Timer? _timer = null;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ValidateParameters();

            int initialDelay = StartAnimationAfterXSec * 1_000;
            int updateInterval = 1_000;
            _timer = new Timer(IncrementPercentage, null, initialDelay, updateInterval);
        }

        private void IncrementPercentage(object? state)
        {
            if (LoadingPercentage == 0)
            {
                Message = "It looks like this might take a while... Time to call in the reinforcements";
                LoadingPercentage = StartAnimationAfterXSec * 100.0 / EndAnimationAfterXSec;
            }
            else if (LoadingPercentage < 100)
            {
                LoadingPercentage += Increment;
            }
            else
            {
                _timer!.Dispose();
            }
            InvokeAsync(StateHasChanged);
        }

        private void ValidateParameters()
        {
            if (StartAnimationAfterXSec < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(StartAnimationAfterXSec), "StartAnimationAfterXSec must be greater than or equal to 1");
            }

            if (EndAnimationAfterXSec < StartAnimationAfterXSec)
            {
                throw new ArgumentOutOfRangeException(nameof(EndAnimationAfterXSec), "EndAnimationAfterXSec must be greater than or equal to StartAnimationAfterXSec");
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
