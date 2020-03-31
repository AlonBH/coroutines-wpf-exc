using System;
using System.Timers;
using Caliburn.Micro;

namespace CoroutinesAssignment.ViewModels
{
    public class GameViewModel : Screen, IResult
    {
        private readonly IWindowManager _windowManager;

        private const int GAME_TIME_LIMIT = 7500;
        private const int GAME_MIN_RANGE = 1;
        private const int GAME_MAX_RANGE = 6;

        private int _numberOfClicks;
        private Timer gameTimer;

        private int _clickObjective;
        public int ClickObjective
        {
            get { return _clickObjective; }
            set { _clickObjective = value; }
        }

        private void CheckGameFinished()
        {
            if (_numberOfClicks == _clickObjective)
            {
                gameTimer.Stop();
                TryClose();

                Completed(this, new ResultCompletionEventArgs());
            }
        }

        public GameViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            _numberOfClicks = 0;
        }

        public void ClickedButton()
        {
            _numberOfClicks++;

            CheckGameFinished();
        }

        protected override void OnActivate()
        {
            _clickObjective = new Random().Next(GAME_MIN_RANGE, GAME_MAX_RANGE);

            base.OnActivate();
        }

        public void Execute(CoroutineExecutionContext context)
        {
            gameTimer = new Timer(GAME_TIME_LIMIT);

            gameTimer.Elapsed += timerOnElapsed(context);
            gameTimer.Start();

            _windowManager.ShowWindow(this);
        }

        private ElapsedEventHandler timerOnElapsed(CoroutineExecutionContext context)
        {
            return (sender, e) =>
            {
                if (_numberOfClicks < _clickObjective)
                {
                    Completed(this, new ResultCompletionEventArgs
                    {
                        WasCancelled = true
                    });

                    (context.Target as ShellViewModel).Result = "על הפנים";
                    gameTimer.Stop();
                    TryClose();
                }
            };
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;
    }
}