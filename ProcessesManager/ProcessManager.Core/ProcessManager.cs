using ProcessManager.Core.Models;
using ProcessManager.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core
{
    public class ProcessManager //: LockedObject
    {
        List<Process> _processes;

        //public int NumberOfThreads
        //{
        //    get
        //    {
        //        return (int)GetValue(nameof(NumberOfThreads));
        //    }
        //    set
        //    {
        //        if (value < 1)
        //            throw new InvalidOperationException("Количество потоков не должно быть меньше одного");
        //        SetValue(nameof(NumberOfThreads), value);
        //    }
        //}

        public IEnumerable<Process> Processes
        {
            get
            {
                return _processes.AsReadOnly();
            }
        }

        public ProcessManager()
        {
            //NumberOfThreads = 2;
        }

        public async Task<Report> StartAsync(IEnumerable<Process> processes, int numberOfThreads)
        {
            return await Task.Run(() => Start(processes, numberOfThreads));
        }

        public Report Start(IEnumerable<Process> processes, int numberOfThreads)
        {
            var res = new Report(numberOfThreads);

            foreach(var process in processes)
            {
                process.Status = ProcessStatus.Wait;
            }
            var uncompletedProcesses = processes.Where(p => !p.Completed);
            var processesInWork = uncompletedProcesses.Where(p => p.Status == ProcessStatus.Work);
            var toWorkCandidates = uncompletedProcesses.Where(p=>p.Status == ProcessStatus.Wait);
            var waitings = processes.ToList();
            var allCount = waitings.Count;
            var threads = new Process[numberOfThreads];
            var hdd = new List<Process>();
            var completed = new List<Process>();
            while (allCount > completed.Count)
            {
                // Процессы в работе выполняют работу (время текущего состояния уменьшается)
                foreach (var process in processesInWork)
                {
                    var tickResult = process.OnTick();
                    if (tickResult.Completed)
                    {
                        RemoveProcessFromthreads(threads, process);
                        completed.Add(process);
                    }
                        
                }

                // Затыкаем дыры в работе приоритетными процессами
                foreach (var process in toWorkCandidates.OrderByDescending(p => p.Prioritet))
                {
                    if (!PutProcessToEmptyThread(threads, process))
                        break;
                }

                // Проверяем, не ожидают ли выполнения более приоритетные процессы (если, что заменяем)
                foreach (var process in toWorkCandidates.OrderByDescending(p => p.Prioritet))
                {
                    if (ReplaceProcessToMoreHighPrioritet(threads, process))
                        break;
                }
            }
            return res;
        }

        static void RemoveProcessFromthreads(Process[] threads, Process process)
        {
            for (var i = 0; i < threads.Length; i++)
                if (threads[i] == process)
                    threads[i] = null;
        }

        static bool PutProcessToEmptyThread(Process[] threads, Process process)
        {
            for (var i = 0; i < threads.Length; i++)
                if (threads[i] == null)
                {
                    threads[i] = process;
                    process.Status = ProcessStatus.Work;
                    return true;
                }
            return false;     
        }

        static bool ReplaceProcessToMoreHighPrioritet(Process[] threads, Process process, out Process replaced)
        {
            for (var i = 0; i < threads.Length; i++)
            {
                if (threads[i] == null || threads[i].Prioritet < process.Prioritet)
                {
                    replaced = threads[i];
                    threads[i] = process;
                    process.Status = ProcessStatus.Work;
                    if (replaced != null)
                        replaced.Status = ProcessStatus.Wait;
                    return true;
                }
            }
            replaced = null;
            return false;
        }
    }

    public enum ProcessManagerStatus
    {
        Configure,
        Ready,
        Work,
        Wait,
        Completed,
    }
}
