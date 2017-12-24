using ProcessManager.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Models
{
    public class Process //: LockedObject
    {
        List<ProcessStage> _stages;

        ProcessStatus _status;

        public event Action<Process> ProcessCompleted;
        public event Action<Process, ProcessStage> ProcessStageCompleted;

        public int CurrentStageIndex { get; private set; }
        public ProcessStage CurrentStage
        {
            get
            {
                if (Completed)
                    return null;
                return _stages[CurrentStageIndex];
            }
        }
        

        public string ProcessName { get; set; }

        public int Prioritet { get; set; }

        public bool Completed
        {
            get
            {
                return CurrentStageIndex == _stages.Count;
            }
        }

        public ProcessStatus Status
        {
            get
            {
                if (Completed)
                    return ProcessStatus.Completed;
                return _status;
            }
            set
            {
                _status = value;
            }
        }
            

        public IEnumerable<ProcessStage> Stages
        {
            get
            {
                return _stages.AsReadOnly();
            }
        }

        public Process(int cpu1Time, int hdd2Time, int cpu3Time)
        {
            _stages = new List<ProcessStage>
            {
                new ProcessStage(cpu1Time, ProcessStages.CPU),
                new ProcessStage(hdd2Time, ProcessStages.HDD),
                new ProcessStage(cpu3Time, ProcessStages.CPU),
            };

            //Status = ProcessStatus.Configure;
        }

        public IEnumerable<ProcessStage> AddStages(int hddTime, int cpuTime)
        {
            var res = new List<ProcessStage>
            {
                new ProcessStage(hddTime, ProcessStages.HDD),
                new ProcessStage(cpuTime, ProcessStages.CPU),
            };
            _stages.AddRange(res);
            return res.AsReadOnly();
        }

        public void RemoveStages(int firstIndex)
        {
            if (_stages.Count < 5)
                throw new InvalidOperationException("Количество этапов не может быть меньше 3");
            _stages.RemoveRange(firstIndex, 2);
        }

        internal ProcessTickResult OnTick()
        {
            switch (Status)
            {
                case ProcessStatus.Work:
                    var res = CurrentStage.OnTick();
                    if (res.Completed)
                    {
                        CurrentStageIndex++;
                    }
                    break;
            }
            if (Completed)
            {
                return new ProcessTickResult(true);
            }
            else
            {
                return new ProcessTickResult(false, CurrentStage.LeftTime);
            }
        }
    }

    public enum ProcessStatus
    {
        Work,
        Wait,
        Completed,
    }

    public class ProcessTickResult
    {
        public bool Completed { get; }
        public int CurrentStageTimeLeft { get; }

        public ProcessTickResult(bool completed)
        {
            Completed = completed;
        }

        public ProcessTickResult(bool completed, int currentStageTimeLeft)
        {
            Completed = completed;
            CurrentStageTimeLeft = currentStageTimeLeft;
        }
    }
}
