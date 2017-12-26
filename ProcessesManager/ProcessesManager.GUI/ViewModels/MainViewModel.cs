using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProcessesManager.GUI.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand StartCommand { get; }
        public RelayCommand OpenProcessesWindowCommand { get; }

        public MainViewModel()
        {
            StartCommand = new RelayCommand(StartExecute, StartCanExecute);
            OpenProcessesWindowCommand = new RelayCommand(OpenProcessesExecute, OpenProcessesCanExecute);
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
                    row[columnIndex++] = reportRow.Time;
                    row[columnIndex++] = reportRow.WaitingProcesses;

                    foreach (var process in reportRow.WorkingProcesses)
                        row[columnIndex++] = process;

                    row[columnIndex++] = reportRow.HddProcesses;

                    res.Rows.Add(row);
                }
                return res;
            }
        }

        List<DataColumn> DataColumns
        {
            get
            {
                var res = new List<DataColumn>();
                res.Add(new DataColumn("Время", typeof(int)));
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

        void StartExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        bool OpenProcessesCanExecute(object parameter)
        {
            return !Processing;
        }

        void OpenProcessesExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
