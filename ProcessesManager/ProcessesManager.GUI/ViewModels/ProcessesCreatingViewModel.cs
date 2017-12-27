using Microsoft.Win32;
using ProcessesManager.GUI.Classes;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ProcessesManager.GUI.ViewModels
{
    public class ProcessesCreatingViewModel : ViewModel
    {
        public RelayCommand AddProcessCommand { get; }
        public RelayCommand RemoveProcessCommand { get; }
        public RelayCommand ImportCommand { get; }
        public RelayCommand ExportCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand GenerateCommand { get; }

        public ObservableCollection<ProcessViewModel> Processes
        {
            get { return GetValue<ObservableCollection<ProcessViewModel>>(); }
            set { SetValue(value); }
        }

        public ProcessesGeneratorViewModel Generator { get; } = new ProcessesGeneratorViewModel();

        public ProcessesCreatingViewModel()
        {
            AddProcessCommand = new RelayCommand(CreateProcess, CanExecuteTrue);
            RemoveProcessCommand = new RelayCommand(RemoveProcess, CanExecuteTrue);
            ExportCommand = new RelayCommand(Export, CanExport);
            ImportCommand = new RelayCommand(Import, CanExecuteTrue);
            ClearCommand = new RelayCommand(Clear, CanClear);
            GenerateCommand = new RelayCommand(Generate, CanExecuteTrue);
        }

        void CreateProcess(object parameter)
        {
            var name = ChangeName();
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
            if (!IsValid)
            {
                ShowValidationDetailsMessage();
                return;
            }
            try
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
            catch (Exception ex)
            {
                var message = "Попытка экспорта в файл завершилась ошибкой:";
                var currentEx = ex;
                while (currentEx != null)
                {
                    message = $"{message}\n{ex.Message}";
                    currentEx = ex.InnerException;
                }
                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Import(object parameter)
        {
            try
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
            catch(Exception ex)
            {
                var message = "Попытка импорта из файла завершилась ошибкой:";
                var currentEx = ex;
                while(currentEx != null)
                {
                    message = $"{message}\n{ex.Message}";
                    currentEx = ex.InnerException;
                }
                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        //bool CanGenerate(object parameter)
        //{
        //    return Generator.IsValid;
        //}

        void Generate(object parameter)
        {
            if (!Generator.IsValid)
            {
                //foreach (var det in Generator.ValidationDetails)
                //    ValidationDetails.Add(det);
                Generator.ShowValidationDetailsMessage();
                return;
            }
            for (var i = 0; i < Generator.ProcessesCount; i++)
            {
                var process = new ProcessViewModel
                {
                    Name = ChangeName(),
                    Prioritet = RandomHelper.Randomizer.Next(Generator.PriorityFrom, Generator.PriorityTo+1),
                };

                var stagesCount = RandomHelper.Randomizer.Next(Generator.StagesCountFrom, Generator.StagesCountTo+1);
                var coercedStagesCount = stagesCount % 2 == 1 ? stagesCount : stagesCount - 1;
                for (var j = 0; j < coercedStagesCount; j++)
                {
                    var stage = new StageViewModel
                    {
                        Time = RandomHelper.Randomizer.Next(Generator.StageTimeFrom, Generator.StageTimeTo+1),
                    };
                    process.Stages.Add(stage);
                }
                Processes.Add(process);
            }
        }

        /// <summary>
        /// Генирируем имя процесса по шаблону Пр_{порядковый номер}. 
        /// </summary>
        /// <returns></returns>
        string ChangeName()
        {
            var counter = 0;
            var name = $"Пр_{++counter}";
            while (Processes.Any(p => p.Name == (name = $"Пр_{counter++}"))) { }
            return name;
        }

        protected override bool Validate()
        {
            var res = true;
            ValidationDetails.Clear();
            for (var i = 0; i < Processes.Count; i++)
            {
                for (var j = i + 1; j < Processes.Count; j++)
                {
                    if (Processes[i].Name.ToLower() == Processes[j].Name.ToLower())
                    {
                        res = false;
                        ValidationDetails.Add($"Имя процесса с индексом {i} совпадает с именем процесса с индексом {j}");
                    }
                }

                if (!Processes[i].IsValid)
                {
                    res = false;
                    foreach (var det in Processes[i].ValidationDetails)
                        ValidationDetails.Add($"Процесс [{i}]{det}");
                }
            }
            return res;
        }
    }
}
