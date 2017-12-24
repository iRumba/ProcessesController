using ProcessManager.Core.Extensions;
using ProcessManager.Core.Models;
using ProcessManager.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core
{
    public class Manager
    {
        public async Task<Report> StartAsync(IEnumerable<Process> processes, int numberOfThreads)
        {
            return await Task.Run(() => Start(processes, numberOfThreads));
        }

        public Report Start(IEnumerable<Process> processes, int numberOfThreads)
        {
            var res = new Report(numberOfThreads);

            var waitings = processes.ToList();
            var allCount = waitings.Count;
            var threads = new Process[numberOfThreads];
            var hdd = new List<Process>();
            var inWork = hdd.Concat(threads).Where(p => p != null);
            var completed = new List<Process>();
            var counter = 0;
            res.AddRow(counter, waitings.ToReport(), threads.ToReport(), hdd.ToReport());
            while (allCount > completed.Count)
            {
                // Процессы в работе выполняют работу (время текущего состояния уменьшается)
                foreach (var process in inWork)
                {
                    var tickResult = process.OnTick();
                    if (tickResult.Completed)
                    {
                        RemoveProcessFromthreads(threads, process);
                        completed.Add(process);
                    }
                    else if (tickResult.StageCompleted)
                    {
                        RemoveProcessFromthreads(threads, process);
                        var list = process.CurrentStage.Stage == ProcessStages.CPU ? waitings : hdd;
                        list.Add(process);
                    }
                }

                // Затыкаем дыры в работе приоритетными процессами
                var toCpu = waitings.Where(p => p.CurrentStage.Stage == ProcessStages.CPU).OrderByDescending(p=>p.Prioritet);
                foreach(var process in toCpu)
                {
                    if (!PutProcessToEmptyThread(threads, process))
                        break;
                    else
                    {
                        waitings.Remove(process);
                    }
                }

                // Проверяем, не ожидают ли выполнения более приоритетные процессы (если, что заменяем)
                foreach (var process in toCpu)
                {
                    if (!PutOrReplaceProcessWithMoreHighPrioritet(threads, process, out var replaced))
                        break;
                    else
                    {
                        waitings.Remove(process);
                        if (replaced != null)
                            waitings.Add(replaced);
                    }
                }

                counter++;
            }
            res.AddRow(counter, waitings.ToReport(), threads.ToReport(), hdd.ToReport());
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
                    return true;
                }
            return false;     
        }

        static bool PutOrReplaceProcessWithMoreHighPrioritet(Process[] threads, Process process, out Process replaced)
        {
            for (var i = 0; i < threads.Length; i++)
            {
                if (threads[i] == null || threads[i].Prioritet < process.Prioritet)
                {
                    replaced = threads[i];
                    threads[i] = process;
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
