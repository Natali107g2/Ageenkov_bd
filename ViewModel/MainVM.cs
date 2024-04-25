using AgeenkovApp.Model;
using AgeenkovApp.View;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace AgeenkovApp.ViewModel {
    public class MainVM : PropChange {
        AggContext db = new AggContext();
        #region gets and sets
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Area> Areas { get; set; }

        public RelayCommand AddNewCustomerCommand { get; set; }
        public RelayCommand EditCustomerCommand { get; set; }
        public RelayCommand DeleteCustomerCommand { get; set; }
        public RelayCommand AddNewProjectCommand { get; set; }
        public RelayCommand EditProjectCommand { get; set; }
        public RelayCommand DeleteProjectCommand { get; set; }
        public RelayCommand AddNewAreaCommand { get; set; }
        public RelayCommand EditAreaCommand { get; set; }
        public RelayCommand DeleteAreaCommand { get; set; }
        public RelayCommand OpenAreaCommand { get; set; }
        #endregion
        public MainVM() {
            db.Database.EnsureCreated();
            db=AggContext.LoadAll();
            Customers = db.Customers.Local.ToObservableCollection();
            Projects = db.Projects.Local.ToObservableCollection();
            Areas = db.Areas.Local.ToObservableCollection();

            AddNewCustomerCommand = new(AddNewCustomer);
            DeleteCustomerCommand = new(DeleteCustomer, (obj) => SelectedCustomer != null);
            AddNewProjectCommand = new(AddNewProject, (obj) => SelectedCustomer != null);
            DeleteProjectCommand = new(DeleteProject, (obj) => SelectedProject != null);
            AddNewAreaCommand = new(AddNewArea, (obj) => SelectedProject != null);
            DeleteAreaCommand = new(DeleteArea, (obj) => SelectedArea != null);
            OpenAreaCommand = new(OpenArea);
        }

        #region Selections
        private Customer selectedCustomer;
        public Customer SelectedCustomer {
            get => selectedCustomer;
            set { selectedCustomer = value; OnPropertyChanged(nameof(SelectedCustomer)); Redraw(); }
        }

        private Project selectedProject;
        public Project SelectedProject {
            get => selectedProject;
            set { selectedProject = value; OnPropertyChanged(nameof(SelectedProject)); Redraw(); }
        }

        private Area selectedArea;
        public Area SelectedArea {
            get => selectedArea;
            set { selectedArea = value; OnPropertyChanged(nameof(SelectedArea)); Redraw(); }
        }
        #endregion

        #region Add Commands
        void AddNewCustomer(object obj) {
            var customer = new Customer() { Name = "", Email = "" };
            if(new NewCustomerWindow(customer).ShowDialog() == false) { return; } else if(customer.Name == "" || customer.Email == "") {
                MessageBox.Show("NEW CUST Заполните все поля!", "Предупреждение");
            } else {
                db.Customers.Add(customer);
                db.SaveChanges();
                SelectedCustomer = customer;
            }
        }

        void AddNewProject(object obj) {
            if(SelectedCustomer == null) { return; }
            var project = new Project() { Name = "", Address = "", Customer = SelectedCustomer };
            if(new NewProjectWindow(project).ShowDialog() == false) { return; } else if(project.Name == "" || project.Address == "") {
                MessageBox.Show("NEW PROJECT Заполните все поля!", "Предупреждение");
            } else {
                db.Projects.Add(project);
                db.SaveChanges();
                SelectedProject = project;
                OnPropertyChanged(nameof(SelectedProject));
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        void AddNewArea(object obj) {
            if(SelectedProject == null) { return; }
            var area = new Area() { Project = SelectedProject };
            db.Areas.Add(area);
            db.SaveChanges();
            area.Name = $"Площадь {area.Id}";
            db.SaveChanges();
            OnPropertyChanged(nameof(SelectedProject));
            SelectedArea = area;
        }
        #endregion

        #region Delete Commands
        void DeleteCustomer(object obj) {
            if(SelectedCustomer == null) { return; }

            if(MessageBox.Show("Are you sure?", "Delete customer",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                db.Customers.Remove(SelectedCustomer);
                db.SaveChanges();
            }
        }

        void DeleteProject(object obj) {
            if(SelectedProject == null) { return; }
            if(MessageBox.Show("Are you sure?", "Delete project",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                db.Projects.Remove(SelectedProject);
                db.SaveChanges();
            }
        }

        void DeleteArea(object obj) {
            if(SelectedArea == null) { return; }
            if(MessageBox.Show("Are you sure?", "Delete area",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                db.Areas.Remove(SelectedArea);
                db.SaveChanges();
                OnPropertyChanged(nameof(SelectedProject));
                db.Areas.Load();
            }
        }
        #endregion
        void OpenArea(object obj) {
            new AreasWindows() {
                DataContext = new AreaVM((Area)obj)
            }.ShowDialog();
            OnPropertyChanged(nameof(SelectedProject.Areas));
            OnPropertyChanged(nameof(obj));
        }

        private DrawingImage image;
        public DrawingImage Image {
            get => image;
            set { image = value; OnPropertyChanged(nameof(Image)); }
        }

        void Redraw() {
            var newimage = new DrawModel();
            foreach(var area in SelectedProject?.Areas ?? new()) {
                if(area == SelectedArea && area.IsCorrect()) area.Draw(newimage, Brushes.Black);
                else if(area == SelectedArea && !area.IsCorrect()) area.Draw(newimage, Brushes.Orange);
                else if(area != SelectedArea && area.IsCorrect()) area.Draw(newimage, Brushes.Green);
                else if(area != SelectedArea && !area.IsCorrect()) area.Draw(newimage, Brushes.Red);
                foreach(var profile in area.Profiles ?? new()) {
                    if(area == SelectedArea && profile.IsCorrect()) profile.Draw(newimage, Brushes.Black);
                    else if(area == SelectedArea && !profile.IsCorrect()) profile.Draw(newimage, Brushes.Red);
                    else if(area != SelectedArea && profile.IsCorrect()) profile.Draw(newimage, Brushes.Green);
                    else if(area != SelectedArea && !profile.IsCorrect()) profile.Draw(newimage, Brushes.IndianRed);
                }
            }
            Image = newimage.Render();
        }
    }
}
