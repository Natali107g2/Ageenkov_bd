using AgeenkovApp.Model;
using AgeenkovApp.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace AgeenkovApp.View {
    public partial class NewMeasuring : Window {
        public NewMeasuring(Measuring measuring) {
            InitializeComponent();
            DataContext = measuring;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }
    }
}
