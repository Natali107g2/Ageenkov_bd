using AgeenkovApp.Model;
using AgeenkovApp.View;
using System.Windows;

namespace AgeenkovApp.ViewModel {
    public class ConnectVM : PropChange {
        public RelayCommand ConnectToDB { get; set; }
        public ConnectVM() {
            ConnectToDB = new(ConnectToDatabase);
        }

        private string dataBaseName;
        public string DataBaseName {
            get => dataBaseName;
            set { dataBaseName = value; OnPropertyChanged(nameof(DataBaseName)); }
        }

        private void ConnectToDatabase(object obj) {
            if(!string.IsNullOrWhiteSpace(DataBaseName)) {
                using(var db = new AggContext(DataBaseName.ToString())) {
                    bool exists = db.Database.CanConnect();
                    if(exists) {
                        MessageBox.Show($"База данных {DataBaseName} существует, подключаемся.", "Подключение");
                    } else {
                        db.Database.EnsureCreated();
                        MessageBox.Show($"Создана новая база данных {DataBaseName}, подключаемся.", "Подключение");
                    }
                    var win = new MainWindow() {
                        DataContext = new MainVM()
                    };
                    win.Show();
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = win;
                    OnPropertyChanged(nameof(obj));
                }
            }
        }
    }
}
