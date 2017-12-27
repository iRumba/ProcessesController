using ProcessesManager.GUI.ViewModels;
using System.Windows;

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

        private void thisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ViewModel.IsValid)
            {
                var message = string.Empty;
                foreach (var det in ViewModel.ValidationDetails)
                    message = $"{message}\n{det}";
                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }
    }
}
