namespace ProcessesManager.GUI.ViewModels
{
    public class ProcessesGeneratorViewModel : ViewModel
    {
        public int ProcessesCount
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int PriorityFrom
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int PriorityTo
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int StagesCountFrom
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int StagesCountTo
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int StageTimeFrom
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int StageTimeTo
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public ProcessesGeneratorViewModel()
        {
            ProcessesCount = 3;
            PriorityFrom = 1;
            PriorityTo = 10;
            StagesCountFrom = 3;
            StagesCountTo = 5;
            StageTimeFrom = 1;
            StageTimeTo = 5;
        }

        protected override bool Validate()
        {
            var res = true;
            ValidationDetails.Clear();
            if (ProcessesCount < 1)
            {
                res = false;
                ValidationDetails.Add("Количество процессов должно быть больше 0");
            }
            if (PriorityFrom < 1)
            {
                res = false;
                ValidationDetails.Add("Минимальное значение приоритета процессов должно быть больше 0");
            }
            if (PriorityTo < PriorityFrom)
            {
                res = false;
                ValidationDetails.Add("Максимальное значение приоритета процессов должно быть больше минимального");
            }
            if (StagesCountFrom < 3)
            {
                res = false;
                ValidationDetails.Add("Минимальное значение количества этапов должно быть больше 3");
            }
            if (StagesCountTo < StagesCountFrom)
            {
                res = false;
                ValidationDetails.Add("Максимальное значение количества этапов должно быть больше минимального");
            }
            if (StageTimeFrom < 1)
            {
                res = false;
                ValidationDetails.Add("Минимальное значение времени этапа должно быть больше 0");
            }
            if (StagesCountTo < StageTimeFrom)
            {
                res = false;
                ValidationDetails.Add("Максимальное значение времени этапа должно быть больше минимального");
            }
            return res;
        }
    }
}
