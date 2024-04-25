using AgeenkovApp.ViewModel;
using System.Windows;

namespace AgeenkovApp.View {
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window {
        public ConnectWindow() {
            InitializeComponent();
            DataContext = new ConnectVM();
        }
    }
}
