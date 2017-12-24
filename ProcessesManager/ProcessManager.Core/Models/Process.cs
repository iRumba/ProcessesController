using ProcessManager.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Models
{
    public class Process
    {
        List<ProcessStage> _stages;

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


        public IEnumerable<ProcessStage> Stages
        {
            get
            {
                return _stages.AsReadOnly();
            }
        }

        public Process(int prioritet, int cpu1Time, int hdd2Time, int cpu3Time)
        {
            _stages = new List<ProcessStage>
            {
                new ProcessStage(cpu1Time, ProcessStages.CPU),
                new ProcessStage(hdd2Time, ProcessStages.HDD),
                new ProcessStage(cpu3Time, ProcessStages.CPU),
            };
            Prioritet = prioritet;
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
            var res = new ProcessTickResult();
            var stageResult = CurrentStage.OnTick();

            if (stageResult.Completed)
            {
                res.StageCompleted = true;
                CurrentStageIndex++;
            }

            if (Completed)
            {
                res.Completed = true;
            }
            else
            {
                res.CurrentStageTimeLeft = CurrentStage.EllapsedTime;
            }
            return res;
        }
    }

    public class ProcessTickResult
    {
        public bool Completed { get; set; }
        public int CurrentStageTimeLeft { get; set; }
        public bool StageCompleted { get; set; }
    }
}
