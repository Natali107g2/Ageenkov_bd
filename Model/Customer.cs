using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AgeenkovApp.Model {
    public class Customer : PropChange {
        private int id;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string? name;
        public string? Name {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string email;
        public string Email {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects {
            get => projects;
            set { projects = value; OnPropertyChanged(nameof(Projects)); }
        }
    }
}
