using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities
{
    public class CountdownTimer
    {
        public bool IsActive { get; private set; }
        
        private static int s_seconds;
        private static float s_waitSecondsBeforeComplete;
        private CancellationTokenSource _cancellationTokenSource;

        public CountdownTimer(int seconds)
        {
            s_seconds = seconds;
        }

        public CountdownTimer(int seconds, float waitSecondsBeforeComplete)
        {
            s_seconds = seconds;
            s_waitSecondsBeforeComplete = waitSecondsBeforeComplete;
        }

        public async void StartCountdown(Action<int> updateOnSecond, Action onComplete)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                IsActive = true;

                for (int remainingSeconds = s_seconds; remainingSeconds > 0; remainingSeconds--)
                {
                    updateOnSecond(remainingSeconds);
                    await Task.Delay(1000, _cancellationTokenSource.Token); // Delay for 1 second
                }

                updateOnSecond(0);

                if (s_waitSecondsBeforeComplete != 0f)
                    //Debug.Log($"Waiting additionally for {(int)(s_waitSecondsBeforeComplete * 1000)}ms");
                    await Task.Delay((int)(s_waitSecondsBeforeComplete * 1000), _cancellationTokenSource.Token);

                //Debug.Log("Countdown complete.");
                onComplete();
            }
            catch (TaskCanceledException)
            {
                //Debug.Log("Countdown canceled.");
                IsActive = false;
            }
        }

        public void CancelCountdown()
        {
            if (_cancellationTokenSource == null) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}