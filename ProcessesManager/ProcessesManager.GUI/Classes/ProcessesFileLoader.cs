using Newtonsoft.Json;
using ProcessesManager.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.Classes
{
    public static class ProcessesFileLoader
    {
        public static IEnumerable<ProcessViewModel> ImportProcesses(string fileName)
        {
            using (var sr = new StreamReader(fileName))
            using (var jr = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<ProcessViewModel>>(jr);
            }
        }

        public static void ExportProcesses(List<ProcessViewModel> processes, string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            using (var jw = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(jw, processes);
            }
        }
    }
}
