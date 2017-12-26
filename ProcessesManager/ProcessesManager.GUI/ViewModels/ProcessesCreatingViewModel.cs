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

        public ObservableCollection<ProcessViewModel> Processes
        {
            get { return GetValue<ObservableCollection<ProcessViewModel>>(); }
            set { SetValue(value); }
        }

        public ProcessesCreatingViewModel()
        {
            AddProcessCommand = new RelayCommand(CreateProcess, (o) => true);
            RemoveProcessCommand = new RelayCommand(RemoveProcess, (o) => true);
        }

        void CreateProcess(object parameter)
        {
            var process = new ProcessViewModel { Prioritet = 1 };
            process.Stages.Add(new StageViewModel { Time = 1 });
            Processes.Add(process);
        }

        void RemoveProcess(object parameter)
        {
            var process = (ProcessViewModel)parameter;
            Processes.Remove(process);
        }
    }
}
