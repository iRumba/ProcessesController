using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Models
{
    public class ReportProcess
    {
        public string Name { get; }
        public int Stage { get; }
        public int Prioritet { get; }
        public int TimeToEnd { get; }

        public ReportProcess(string name, int stage, int prioritet, int timeToEnd)
        {
            Name = name;
            Stage = stage;
            Prioritet = prioritet;
            TimeToEnd = timeToEnd;
        }
    }
}
