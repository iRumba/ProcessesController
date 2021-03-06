﻿using ProcessesManager.GUI.Views;
using ProcessManager.Core;
using ProcessManager.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace ProcessesManager.GUI.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand StartCommand { get; }
        public RelayCommand OpenProcessesWindowCommand { get; }

        public MainViewModel()
        {
            NumberOfThreads = 3;
            StartCommand = new RelayCommand(StartExecute, StartCanExecute);
            OpenProcessesWindowCommand = new RelayCommand(OpenProcessesExecute, OpenProcessesCanExecute);
        }

        public ObservableCollection<ProcessViewModel> Processes { get; } = new ObservableCollection<ProcessViewModel>();

        public int NumberOfThreads
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [Dependencies(nameof(ReportTable))]
        public Report Report
        {
            get { return GetValue<Report>(); }
            set { SetValue(value); }
        }

        public bool Processing
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Здесь я перегоняю тип Report в тип DataTable для удобства отображения в GridView.
        /// Это нужно, так как количество столбцов у нас динамическое, а количество полей в классах C# статическое
        /// и определяется на этапе компиляции
        /// </summary>
        public DataTable ReportTable
        {
            get
            {
                if (Report == null)
                    return null;

                var res = new DataTable();
                res.Columns.AddRange(DataColumns.ToArray());
                foreach(var reportRow in Report.Rows)
                {
                    var row = res.NewRow();

                    var columnIndex = 0;
                    var time = $"{reportRow.Time}";
                    if (reportRow.IsSpecial)
                        time = $"{time}'";
                    row[columnIndex++] = time;
                    row[columnIndex++] = reportRow.WaitingProcesses;

                    foreach (var process in reportRow.WorkingProcesses)
                        row[columnIndex++] = process;

                    row[columnIndex++] = reportRow.HddProcesses;

                    res.Rows.Add(row);
                }
                return res;
            }
        }

        /// <summary>
        /// Здесь генерируются столбцы для DataTable
        /// </summary>
        List<DataColumn> DataColumns
        {
            get
            {
                var res = new List<DataColumn>();
                res.Add(new DataColumn("Время", typeof(string)));
                res.Add(new DataColumn("Готовность", typeof(IEnumerable<ReportProcess>)));
                for (var i = 0; i < Report.NumberOfCpus; i++)
                {
                    res.Add(new DataColumn($"Исполнение {i + 1}", typeof(ReportProcess)));
                }
                res.Add(new DataColumn("Ожидание", typeof(IEnumerable<ReportProcess>)));
                return res;
            }
        }

        bool StartCanExecute(object parameter)
        {
            return !Processing;
        }

        async void StartExecute(object parameter)
        {
            if (!IsValid)
            {
                ShowValidationDetailsMessage();
                return;
            }
            Processing = true;
            var manager = new Manager();
            var report = await manager.StartAsync(Processes.ToModel(), NumberOfThreads);
            Report = report;
            Processing = false;
        }

        bool OpenProcessesCanExecute(object parameter)
        {
            return !Processing;
        }

        void OpenProcessesExecute(object parameter)
        {
            var wnd = new ProcessesManagementView();
            wnd.ViewModel.Processes = Processes;
            wnd.ShowDialog();
        }

        protected override bool Validate()
        {
            var res = true;
            ValidationDetails.Clear();

            if (NumberOfThreads < 1)
            {
                res = false;
                ValidationDetails.Add("Число количества потоков должно быть больше 0");
            }

            if (Processes.Count < 1)
            {
                res = false;
                ValidationDetails.Add("число процессов должно быть больше 0");
            }

            return res;
        }
    }
}
