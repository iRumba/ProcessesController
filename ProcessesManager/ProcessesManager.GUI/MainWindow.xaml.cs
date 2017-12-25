using ProcessesManager.GUI.TemplateSelectors;
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

namespace ProcessesManager.GUI
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
            var processManager = new Manager();
            var processes = new List<Process>
            {
                new Process(1,5,7,9){ ProcessName = "pr1" },
                new Process(2,5,7,9){ ProcessName = "pr2" },
                new Process(3,5,7,9){ ProcessName = "pr3" },
                new Process(4,5,7,9){ ProcessName = "pr4" },
            };
            var res = await processManager.StartAsync(processes, 2);

            Vm.Report = res;
            GenerateColumnsForReport(dgReport);
            //GenerateColumns((GridView)lvMain.View);
        }

        void GenerateColumnsForReport(DataGrid grid)
        {
            grid.Columns.Clear();

            var column1 = new DataGridTextColumn();
            column1.Header = "Время";
            var binding1 = new Binding(nameof(ReportRow.Time));
            

            column1.Binding = binding1;
            grid.Columns.Add(column1);

            var column2 = new DataGridTemplateColumn();
            column2.Header = "В ожидании";
            var style2 = new Style(typeof(DataGridCell));
            style2.Setters.Add(new Setter(DataGridCell.DataContextProperty, new Binding(nameof(ReportRow.WaitingProcesses))));
            style2.Setters.Add(new Setter(DataGridCell.ContentTemplateProperty, (DataTemplate)Resources["ProcessCollectionTemplate"]));
            
            column2.CellStyle = style2;
            //column2.
            //column2.Binding = new Binding(nameof(ReportRow.WaitingProcesses));
            //column2.CellStyle = (Style)Resources["ProcessCollectionStyle"];
            //var dataTemplate2 = (DataTemplate)Resources["ProcessCollectionTemplate"];

            //var d1 = (TestDataTemplate)Resources["qwe"];
            //d1.Prop = 1;
            //var d2 = (TestDataTemplate)Resources["qwe"];
            //var t = dataTemplate2.Template;
            //var itemsControl = (ItemsControl)dataTemplate2.LoadContent();
            //var it = (ItemsControl)dataTemplate2.LoadContent();
            //var b = (itemsControl == it);
            //var d = new DataTemplate();
            //itemsControl.DataContext = new Binding(nameof(ReportRow.WaitingProcesses));
            //var datatemplate2 = new DataTemplate();
            //column2.CellTemplate = dataTemplate2;

            grid.Columns.Add(column2);

            for(var i = 0; i < Vm.Report.NumberOfCpus; i++)
            {
                var column = new DataGridTemplateColumn();
                column.Header = $"Поток {i}";
                column.CellTemplate = (DataTemplate)Resources["ProcessTeplate"];
                
            }

            var column3 = new DataGridTemplateColumn();
            column3.Header = "HDD";
            column3.CellTemplate = (DataTemplate)Resources["HddProcessesCollectionTemplate"];

            grid.Columns.Add(column3);
        }

        void GenerateColumns(GridView view)
        {
            var column1 = new GridViewColumn();
            column1.Header = "Время";
            column1.DisplayMemberBinding = new Binding(nameof(ReportRow.Time));
            view.Columns.Add(column1);

            var column2 = new GridViewColumn();
            column2.Header = "В ожидании";
            column2.DisplayMemberBinding = new Binding(nameof(ReportRow.WaitingProcesses));
            column2.CellTemplateSelector = new ReportColumnsTemplateSelector();// = (DataTemplate)Resources["ProcessCollectionTemplate"];
            view.Columns.Add(column2);
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //if (e.PropertyType == typeof(ReportProcess))
            //{
            //    var col = new DataGridTemplateColumn();
            //    col.CellTemplateSelector = new ReportColumnsTemplateSelector();// .CellTemplate = (DataTemplate)Resources["ProcessTemplate"];
            //    //col.CellTemplate = (DataTemplate)Resources["ProcessTemplate"];
            //    e.Column = col;
            //    //e.Column.
            //    //e.Column.CellStyle = (Style)Resources["ProcessStyle"];
                
            //}
        }
    }
}
