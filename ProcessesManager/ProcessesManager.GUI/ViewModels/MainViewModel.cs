using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Report _report;
        dynamic _table;

        public event PropertyChangedEventHandler PropertyChanged;

        public Report Report
        {
            get
            {
                return _report;
            }
            set
            {
                _report = value;
                OnPropertyChanged(nameof(Report));
                OnPropertyChanged(nameof(ReportTable));
            }
        }

        List<DataColumn> DataColumns
        {
            get
            {
                var res = new List<DataColumn>();
                res.Add(new DataColumn("Время", typeof(int)));
                res.Add(new DataColumn("Ожидание", typeof(IEnumerable<ReportProcess>)));
                for (var i = 0; i < Report.NumberOfCpus; i++)
                {
                    res.Add(new DataColumn($"Поток {i + 1}", typeof(ReportProcess)));
                }
                res.Add(new DataColumn("HDD", typeof(IEnumerable<ReportProcess>)));
                return res;
            }
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

        public dynamic Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
                OnPropertyChanged(nameof(Table));
            }
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
