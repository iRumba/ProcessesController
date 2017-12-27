using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    public static class ViewModelsExtensions
    {
        public static Process ToModel(this ProcessViewModel vm)
        {
            var res = new Process(vm.Prioritet) { ProcessName = vm.Name };
            res.Stages = vm.Stages.ToModel().ToList();
            return res;
        }

        public static IEnumerable<Process> ToModel(this IEnumerable<ProcessViewModel> vms)
        {
            return vms.Select(vm => vm.ToModel());
        }

        public static ProcessStage ToModel(this StageViewModel vm)
        {
            var res = new ProcessStage(vm.Time);
            return res;
        }

        public static IEnumerable<ProcessStage> ToModel(this IEnumerable<StageViewModel> vms)
        {
            return vms.Select(vm => vm.ToModel());
        }
    }
}
