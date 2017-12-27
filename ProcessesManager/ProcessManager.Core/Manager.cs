using ProcessManager.Core.Extensions;
using ProcessManager.Core.Models;
using System.Collections.Generic;
using System.Linq;
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
            var counter = -1;
            res.AddRow(counter, waitings.ToReport(), threads.ToReport(), hdd.ToReport());
            while (allCount > completed.Count)
            {
                var specialFlag = false;
                // Процессы в работе выполняют работу (время текущего состояния уменьшается)
                foreach (var process in inWork.ToList())
                {
                    var tickResult = process.OnTick();
                    if (tickResult.Completed)
                    {
                        RemoveProcessFromthreads(threads, process);
                        completed.Add(process);
                    }
                    else if (tickResult.StageCompleted)
                    {
                        List<Process> listToAdd = null;
                        var stageAlternate = process.CurrentStageIndex % 2;
                        switch (stageAlternate)
                        {
                            case 1:
                                listToAdd = hdd;
                                RemoveProcessFromthreads(threads, process);
                                break;
                            case 0:
                                listToAdd = waitings;
                                hdd.Remove(process);
                                specialFlag = true;
                                break;
                        }
                        RemoveProcessFromthreads(threads, process);
                        listToAdd.Add(process);

                    }
                }
                counter++;
                if (specialFlag)
                    res.AddRow(counter, waitings.ToReport(), threads.ToReport(), hdd.ToReport(), true);

                var removeSpecialFlag = true;
                // Затыкаем дыры в работе приоритетными процессами
                var toCpu = waitings.Where(p => p.CurrentStageIndex % 2 == 0).OrderByDescending(p=>p.Prioritet);
                foreach(var process in toCpu)
                {
                    if (!PutProcessToEmptyThread(threads, process))
                        break;
                    else
                    {
                        removeSpecialFlag = false;
                        waitings.Remove(process);
                    }
                }

                // Проверяем, не ожидают ли выполнения более приоритетные процессы (если, что заменяем)
                foreach (var process in toCpu)
                {
                    if (!ReplaceProcessWithMoreHighPrioritet(threads, process, out var replaced))
                        break;
                    else
                    {
                        removeSpecialFlag = false;
                        waitings.Remove(process);
                        if (replaced != null)
                            waitings.Add(replaced);
                    }
                }

                if (specialFlag && removeSpecialFlag)
                    res.RemoveLast();
                
                res.AddRow(counter, waitings.ToReport(), threads.ToReport(), hdd.ToReport());
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
                    return true;
                }
            return false;     
        }

        static bool ReplaceProcessWithMoreHighPrioritet(Process[] threads, Process process, out Process replaced)
        {
            var minIndex = -1;
            var minPriority = int.MaxValue;
            for (var i = 0; i < threads.Length; i++)
            {
                if (threads[i] != null && threads[i].Prioritet < minPriority)
                {
                    minPriority = threads[i].Prioritet;
                    minIndex = i;
                }
            }
            if (minPriority > 0 && minPriority < process.Prioritet)
            {
                replaced = threads[minIndex];
                threads[minIndex] = process;
                return true;
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
