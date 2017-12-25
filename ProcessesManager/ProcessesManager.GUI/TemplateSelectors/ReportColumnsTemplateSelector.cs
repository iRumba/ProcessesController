using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProcessesManager.GUI.TemplateSelectors
{
    public class ReportColumnsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ProcessTemplate { get; set; }
        public DataTemplate ProcessCollectionTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ReportProcess)
                return ProcessTemplate;
            else if (item is IEnumerable<ReportProcess>)
                return ProcessCollectionTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
