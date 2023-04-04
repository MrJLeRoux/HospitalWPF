using System.Windows;

namespace HospitalWPF
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new ViewModel();
            InitializeComponent();
        }
    }
}
