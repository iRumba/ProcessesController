using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProcessesManager.GUI
{
    public class DataGridProcessContainerColumn : DataGridBoundColumn
    {
        public DataTemplate ContentTemplate { get; set; }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            throw new NotImplementedException();
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var control = new ContentControl();
            control.ContentTemplate = ContentTemplate;
            BindingOperations.SetBinding(control, ContentControl.ContentProperty, Binding);
            return control;
        }
    }
}
