using AgeenkovApp.Model;
using System.Windows;

namespace AgeenkovApp.View {
    public partial class NewCustomerWindow : Window {
        public NewCustomerWindow(Customer customer) {
            InitializeComponent();
            DataContext = customer;
        }

        private void Button_Click(object sender, EventArgs e) {
            DialogResult = true;
        }
    }
}
