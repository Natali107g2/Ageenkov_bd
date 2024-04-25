using System.Collections.ObjectModel;

namespace AgeenkovApp.Model {
    public class Operator : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string name;
        public string Name {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string lastName;
        public string LastName {
            get => lastName;
            set { lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        private string email;
        public string Email {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        private ObservableCollection<Measuring> measuring;
        public ObservableCollection<Measuring> Measuring {
            get => measuring;
            set { measuring = value; OnPropertyChanged(nameof(Measuring)); }
        }
    }
}
