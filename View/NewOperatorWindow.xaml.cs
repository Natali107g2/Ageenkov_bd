using AgeenkovApp.Model;
using System.Windows;

namespace AgeenkovApp.View {
    public partial class NewOperatorWindow : Window {
        public NewOperatorWindow(Operator oper) {
            InitializeComponent();
            DataContext = oper;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }
    }
}
