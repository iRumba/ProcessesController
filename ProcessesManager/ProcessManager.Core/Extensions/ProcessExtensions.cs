using ProcessManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProcessManager.Core.Extensions
{
    public static class ProcessExtensions
    {
        public static ReportProcess ToReport(this Process process)
        {
            if (process == null)
                return null;

            var res = new ReportProcess(process.ProcessName, 
                process.CurrentStageIndex + 1, 
                process.Prioritet, 
                process.CurrentStage.LeftTime);
            return res;
        }

        public static IEnumerable<ReportProcess> ToReport(this IEnumerable<Process> processes)
        {
            var res = processes.Select(p => p?.ToReport());
            return res;
        }
    }
}
