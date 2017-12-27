using ProcessesManager.GUI.ViewModels;
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
using System.Windows.Shapes;

namespace ProcessesManager.GUI.Views
{
    /// <summary>
    /// Логика взаимодействия для ProcessesManagementView.xaml
    /// </summary>
    public partial class ProcessesManagementView : Window
    {
        public ProcessesCreatingViewModel ViewModel
        {
            get
            {
                return DataContext as ProcessesCreatingViewModel;
            }
        }
        public ProcessesManagementView()
        {
            InitializeComponent();
        }
    }
}
