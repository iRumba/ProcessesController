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


        public List<ProcessStage> Stages { get; set; } = new List<ProcessStage>();

        public Process(int prioritet)
        {
            _stages = new List<ProcessStage>();
            Prioritet = prioritet;
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
