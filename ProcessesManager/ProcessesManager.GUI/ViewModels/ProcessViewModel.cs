using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProcessViewModel : ViewModel
    {
        public RelayCommand AddStageCommand { get; }
        public RelayCommand RemoveStageCommand { get; set; }

        [JsonProperty]
        public ObservableCollection<StageViewModel> Stages { get; } = new ObservableCollection<StageViewModel>();

        [JsonProperty]
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [JsonProperty]
        public int Prioritet
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public ProcessViewModel()
        {
            AddStageCommand = new RelayCommand(AddStage, (o) => true);
            RemoveStageCommand = new RelayCommand(RemoveStage, CanRemoveStage);
        }

        bool CanRemoveStage(object parameter)
        {
            return Stages.Count > 4;
        }

        void RemoveStage(object parameter)
        {
            var stage = (StageViewModel)parameter;
            var index = Stages.IndexOf(stage);
            Stages.Remove(stage);
            Stages.RemoveAt(Math.Min(index, Stages.Count - 1));
        }

        void AddStage(object parameter)
        {
            Stages.Add(new StageViewModel { Time = 1 });
            Stages.Add(new StageViewModel { Time = 1 });
        }
    }
}
