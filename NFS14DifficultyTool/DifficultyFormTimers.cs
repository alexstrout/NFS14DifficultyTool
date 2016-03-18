using System;
using System.Timers;

namespace NFS14DifficultyTool {
    public class DifficultyFormTimers {
        protected DifficultyForm parent;
        protected DifficultyFormWorker worker;

        public Timer FindProcessTimer { get; set; }
        public Timer SessionChangeTimer { get; set; }

        public DifficultyFormTimers(DifficultyForm parent, DifficultyFormWorker worker) {
            this.parent = parent;
            this.worker = worker;

            //Initialize our timers
            FindProcessTimer = new Timer(1000);
            FindProcessTimer.Elapsed += FindProcessTimer_Tick;
            SessionChangeTimer = new Timer(6000);
            SessionChangeTimer.Elapsed += SessionChangeTimer_Tick;

            //Start looking for our game
            FindProcessTimer.Start();
        }

        public delegate void TimerCallback();

        private void FindProcessTimer_Tick(object sender, EventArgs e) {
            //Wait until we're ready to go
            if (!worker.CheckIfReady())
                return;

            //All set! No use for timer now (until we have an error or something at least)
            FindProcessTimer.Stop();
            FindProcessTimer.Interval = 1000;

            //Start checking our Matchmaking settings
            SessionChangeTimer.Start();

            //Fire off the rest of the settings events now that we're ready
            parent.Invoke(new TimerCallback(parent.ApplyAllSettings), new object[] { });
        }

        private void SessionChangeTimer_Tick(object sender, EventArgs e) {
            if (worker.GetMatchmakingMode() == MatchmakingModeEnum.Public)
                parent.Invoke(new TimerCallback(parent.ValidateOnlineOptions), new object[] { });
        }
    }
}
