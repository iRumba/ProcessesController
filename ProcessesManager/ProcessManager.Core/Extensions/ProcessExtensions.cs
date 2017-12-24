using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Extensions
{
    public static class ProcessExtensions
    {
        public static ReportProcess ToReport(this Process process)
        {
            if (process == null)
                return null;

            var res = new ReportProcess(process.ProcessName, 
                process.CurrentStageIndex, 
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
