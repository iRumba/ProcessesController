﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Models
{
    public class Report
    {
        List<ReportRow> _rows;
        public int NumberOfCpus { get; }
        public IEnumerable<ReportRow> Rows
        {
            get
            {
                return _rows.OrderBy(r => r.Time);
            }
        }

        public Report(int numberOfCpus)
        {
            _rows = new List<ReportRow>();
            NumberOfCpus = numberOfCpus;
        }

        public void AddRow(ReportRow row)
        {
            if (row.WorkingProcesses.Count() != NumberOfCpus)
                throw new InvalidOperationException("Неверная строка отчета");

            var last = _rows.LastOrDefault();
            _rows.Add(row);
            if (last != null && last.IsSpecial && last.Time == row.Time)
            {
                last.IsSpecial = false;
                _rows.Last().IsSpecial = true;
            }
        }

        public void AddRow(int time, IEnumerable<ReportProcess> waitingProcesses, IEnumerable<ReportProcess> workingProcesses, IEnumerable<ReportProcess> hddProcesses, bool isSpecial = false)
        {
            var row = new ReportRow(time, waitingProcesses, workingProcesses, hddProcesses, isSpecial);
            AddRow(row);
        }

        public void RemoveLast()
        {
            _rows.Remove(_rows.Last());
        }
    }
}
