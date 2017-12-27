using ProcessesManager.GUI.ViewModels;
using ProcessManager.Core;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProcessesManager.GUI.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel Vm
        {
            get
            {
                return (MainViewModel)DataContext;
            }
        }
        public MainWindow()
        {
            InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //var processManager = new Manager();
            //var processes = new List<Process>
            //{
            //    new Process(1,5,7,9){ ProcessName = "pr1" },
            //    new Process(2,5,7,9){ ProcessName = "pr2" },
            //    new Process(3,5,7,9){ ProcessName = "pr3" },
            //    new Process(4,5,7,9){ ProcessName = "pr4" },
            //};
            //var res = await processManager.StartAsync(processes, 2);

            //Vm.Report = res;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType.IsAssignableFrom(typeof(IEnumerable<ReportProcess>)))
            {
                var col = new DataGridProcessContainerColumn();
                col.Binding = (e.Column as DataGridBoundColumn).Binding;
                col.ContentTemplate = (DataTemplate)Resources["ProcessCollectionTemplate"];
                col.Header = e.Column.Header;
                e.Column = col;
            }
            if (e.PropertyType.IsAssignableFrom(typeof(ReportProcess)))
            {
                var col = new DataGridProcessContainerColumn();
                col.Binding = (e.Column as DataGridBoundColumn).Binding;
                col.ContentTemplate = (DataTemplate)Resources["ProcessTemplate"];
                col.Header = e.Column.Header;
                e.Column = col;
            }
        }
    }
}
