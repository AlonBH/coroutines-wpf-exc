using System;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace CoroutinesAssignment.ViewModels
{
    public class PrimeNumbersViewModel : Screen, IResult
    {
        private const int PRIME_SEARCH_LIMIT = 100000;
        // private const int PRIME_SEARCH_LIMIT = 50;

        private readonly IWindowManager _windowManager;

        public PrimeNumbersViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public async Task<int> CountAllPrimeNumbersAsync()
        {
            return await Task.Run(() =>
            {
                var count = 0;

                for (var i = 0; i < PRIME_SEARCH_LIMIT; i++)
                {
                    if (IsPrime(i))
                    {
                        count++;
                    }
                }

                TryClose();

                return count;
            });
        }
        private bool IsPrime(int n)
        {
            for (var i = 2; i <= n / 2; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public async void Execute(CoroutineExecutionContext context)
        {
            var numberOfPrimes = CountAllPrimeNumbersAsync();

            _windowManager.ShowDialog(this);

            ((ShellViewModel) context.Target).Result = (await numberOfPrimes).ToString();
            
            Completed(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;
    }
}
