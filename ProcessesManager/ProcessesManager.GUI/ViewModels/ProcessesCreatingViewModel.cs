using Microsoft.Win32;
using ProcessesManager.GUI.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    public class ProcessesCreatingViewModel : ViewModel
    {
        public RelayCommand AddProcessCommand { get; }
        public RelayCommand RemoveProcessCommand { get; }
        public RelayCommand ImportCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ObservableCollection<ProcessViewModel> Processes
        {
            get { return GetValue<ObservableCollection<ProcessViewModel>>(); }
            set { SetValue(value); }
        }

        public ProcessesCreatingViewModel()
        {
            AddProcessCommand = new RelayCommand(CreateProcess, CanExecuteTrue);
            RemoveProcessCommand = new RelayCommand(RemoveProcess, CanExecuteTrue);
            ExportCommand = new RelayCommand(Export, CanExport);
            ImportCommand = new RelayCommand(Import, CanExecuteTrue);
            ClearCommand = new RelayCommand(Clear, CanClear);
        }

        void CreateProcess(object parameter)
        {
            var counter = 0;
            var name = $"Пр_{++counter}";
            while (Processes.Any(p => p.Name == (name = $"Пр_{counter++}"))) { }
            var process = new ProcessViewModel { Prioritet = 1, Name = name };
            process.Stages.Add(new StageViewModel { Time = 1 });
            process.Stages.Add(new StageViewModel { Time = 1 });
            process.Stages.Add(new StageViewModel { Time = 1 });
            Processes.Add(process);
        }

        void RemoveProcess(object parameter)
        {
            var process = (ProcessViewModel)parameter;
            Processes.Remove(process);
        }

        bool CanExport(object parameter)
        {
            return AnyProcesses();
        }

        void Export(object parameter)
        {
            var dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "txt";
            dlg.Filter = "(Текстовые файлы)|*.txt";
            dlg.OverwritePrompt = true;
            var res = dlg.ShowDialog();
            if (res.HasValue && res.Value)
            {
                var fileName = dlg.FileName;
                ProcessesFileLoader.ExportProcesses(Processes.ToList(), fileName);
            }
        }

        void Import(object parameter)
        {
            var dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "txt";
            dlg.Filter = "(Текстовые файлы)|*.txt";
            dlg.Multiselect = false;

            var res = dlg.ShowDialog();
            if (res.HasValue && res.Value)
            {
                var fileName = dlg.FileName;
                var processes = ProcessesFileLoader.ImportProcesses(fileName);
                Processes.Clear();
                foreach (var process in processes)
                    Processes.Add(process);
            }
        }

        bool CanClear(object parameter)
        {
            return AnyProcesses();
        }

        void Clear(object parameter)
        {
            Processes.Clear();
        }

        bool AnyProcesses()
        {
            return Processes != null && Processes.Count > 0;
        }
    }
}
