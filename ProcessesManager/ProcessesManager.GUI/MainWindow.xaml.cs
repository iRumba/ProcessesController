using ProcessManager.Core;
using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            var processManager = new Manager();
            var processes = new List<Process>
            {
                new Process(1,5,7,9),
                new Process(2,5,7,9),
                new Process(3,5,7,9),
                new Process(4,5,7,9),
            };
            var res = processManager.Start(processes, 2);
        }
    }
}
