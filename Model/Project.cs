using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgeenkovApp.Model {
    public class Project : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string? name;
        public string Name {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string? address;
        public string? Address {
            get => address;
            set { address = value; OnPropertyChanged(nameof(Address)); }
        }

        private Customer customer;
        public Customer Customer {
            get => customer;
            set { customer = value; OnPropertyChanged(nameof(Customer)); }
        }

        private ObservableCollection<Area> areas;
        public ObservableCollection<Area> Areas {
            get => areas;
            set { areas = value; OnPropertyChanged(nameof(Areas)); }
        }
    }
}
