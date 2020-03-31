using System.Collections.Generic;
using Caliburn.Micro;
using CoroutinesAssignment.ViewModels.Interfaces;

namespace CoroutinesAssignment.ViewModels
{ 
    public class ShellViewModel : Screen, IShell
    {
        private readonly IWindowManager _windowManager;

        private string _result;

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                NotifyOfPropertyChange(() => Result);
            }
        }

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public IEnumerable<IResult> StartGame()
        {
            for (var i = 0; i < 3; i++)
            {
                yield return new GameViewModel(_windowManager);
            }

            Result = "כל הכבוד";
        }

        public IEnumerable<IResult> CountPrimes()
        {
            yield return new PrimeNumbersViewModel(_windowManager);
        }

        public void Both()
        {
            Coroutine.BeginExecute(CountPrimes().GetEnumerator(), new CoroutineExecutionContext()
            {
                Target = this
            });
            Coroutine.BeginExecute(StartGame().GetEnumerator());
        }
    }
}