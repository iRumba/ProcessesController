using ProcessManager.Core.Primitives;
using System;

namespace ProcessManager.Core.Models
{
    public class ProcessStage //: LockedObject
    {
        public int EllapsedTime { get; private set; }

        // Здесь на сеттер ограничение. Время должно быть больше либо равно одной секунде
        public int TimeToStage { get; set; }

        public int LeftTime
        {
            get
            {
                return TimeToStage - EllapsedTime;
            }
        }

        // Тип этапа не должен меняться в процессе исполнения программы, поэтому сеттера нет.
        public ProcessStages Stage { get; }

        public ProcessStage(int seconds, ProcessStages stage)
        {
            TimeToStage = seconds;
            Stage = stage;
        }

        internal ProcessStageTickResult OnTick()
        {
            EllapsedTime++;

            if (LeftTime == 0)
                return new ProcessStageTickResult(true);
            return new ProcessStageTickResult(false);
        }
    }

    public enum ProcessStages
    {
        CPU,
        HDD,
    }

    public class ProcessStageTickResult
    {
        public bool Completed { get; }

        public ProcessStageTickResult(bool completed)
        {
            Completed = completed;
        }
    }
}
