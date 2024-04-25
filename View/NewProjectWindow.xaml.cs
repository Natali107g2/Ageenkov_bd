using AgeenkovApp.Model;
using System.Windows;

namespace AgeenkovApp.View {
    public partial class NewProjectWindow : Window {
        public NewProjectWindow(Project project) {
            InitializeComponent();
            DataContext = project;
        }

        private void Button_Click(object sender, EventArgs e) {
            DialogResult = true;
        }
    }
}
