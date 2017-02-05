using System.Windows;
using Weather.Client.Desktop.Windows.ViewModel;

namespace Weather.Client.Desktop.Windows.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _currentConditionsView.DataContext = new ConditionsViewModel();
        }
    }
}