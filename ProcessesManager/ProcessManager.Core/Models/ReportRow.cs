using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Models
{
    public class ReportRow
    {
        public int Time { get; }
        public List<ReportProcess> WaitingProcesses { get; }
        public List<ReportProcess> WorkingProcesses { get; }
        public List<ReportProcess> HddProcesses { get; }
        public bool IsSpecial { get; set; }

        public ReportRow(int time, IEnumerable<ReportProcess> waitingProcesses, IEnumerable<ReportProcess> workingProcesses, IEnumerable<ReportProcess> hddProcesses, bool isSpecial)
        {
            Time = time;
            WaitingProcesses = waitingProcesses.ToList();
            WorkingProcesses = workingProcesses.ToList();
            HddProcesses = hddProcesses.ToList();
            IsSpecial = isSpecial;
        }
    }
}
