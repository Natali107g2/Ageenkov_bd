using AgeenkovApp.Model;
using AgeenkovApp.View;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace AgeenkovApp.ViewModel {
    public class ProfileVM : PropChange {
        AggContext db = AggContext.LoadAll();

        public RelayCommand AddNewOperatorCmd { get; set; }
        public RelayCommand DeleteOperatorCmd { get; set; }
        public RelayCommand AddProfileCoordCmd { get; set; }
        public RelayCommand SaveProfileCoordCmd { get; set; }
        public RelayCommand DeleteProfileCoordCmd { get; set; }
        public RelayCommand AddPicketCmd { get; set; }
        public RelayCommand SavePicketCoordCmd { get; set; }
        public RelayCommand DeletePicketCmd { get; set; }
        public RelayCommand OpenPicketCmd { get; set; }
        public RelayCommand AddMeasuringCmd { get; set; }
        public RelayCommand DeleteMeasuringCmd { get; set; }

        public Profile Profile { get; set; }
        public ObservableCollection<Operator> Operators { get; set; }
        public ObservableCollection<ProfileCoords> ProfileCoords { get; set; }
        public ObservableCollection<Picket> Pickets { get; set; }

        public ProfileVM() : this(new Profile()) { }

        public ProfileVM(Profile profile) {
            Profile = profile;

            Operators = db.Operators.Local.ToObservableCollection();
            ProfileCoords = db.ProfileCoords.Local.ToObservableCollection();
            Pickets = db.Pickets.Local.ToObservableCollection();

            AddNewOperatorCmd = new(AddNewOperator);
            DeleteOperatorCmd = new(DeleteOperator);
            AddProfileCoordCmd = new(AddProfileCoord);
            SaveProfileCoordCmd = new(SaveProfileCoord);
            DeleteProfileCoordCmd = new(DeleteProfileCoord);
            AddPicketCmd = new(AddPicket);
            SavePicketCoordCmd = new(SavePicketCoord);
            DeletePicketCmd = new(DeletePicket);

            OpenPicketCmd = new(OpenPicket);
        }

        private Operator selectedOperator;
        public Operator SelectedOperator {
            get => selectedOperator;
            set { selectedOperator = value; OnPropertyChanged(nameof(SelectedOperator)); }
        }

        private ProfileCoords selectedProfileCoords;
        public ProfileCoords SelectedProfileCoords {
            get => selectedProfileCoords;
            set { selectedProfileCoords = value; OnPropertyChanged(nameof(SelectedProfileCoords)); Redraw(); }
        }

        private Picket selectedPicket;
        public Picket SelectedPicket {
            get => selectedPicket;
            set { selectedPicket = value; OnPropertyChanged(nameof(SelectedPicket)); Redraw(); }
        }

        void AddNewOperator(object obj) {
            var oper = new Operator() { Name = "", LastName = "", Email = "" };
            if(new NewOperatorWindow(oper).ShowDialog() == false) return;
            else if(string.IsNullOrEmpty(oper.Name) || string.IsNullOrEmpty(oper.LastName) || string.IsNullOrEmpty(oper.Email)) { return; } else {
                db.Operators.Add(oper);
                db.SaveChanges();
                SelectedOperator = oper;
            }
        }

        void DeleteOperator(object obj) {
            if(SelectedOperator == null) return;
            if(MessageBox.Show("Are you sure?", "Delete operator",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                db.Operators.Remove(SelectedOperator);
                db.SaveChanges();
            }
        }

        void AddProfileCoord(object obj) {
            var coord = new ProfileCoords() { X = 0, Y = 0, Profile = Profile };
            db.ProfileCoords.Add(coord);
            db.SaveChanges();
            SelectedProfileCoords = coord;
            OnPropertyChanged(nameof(Profile));
        }

        void SaveProfileCoord(object obj) {
            if(obj is ProfileCoords) {
                db.Entry((ProfileCoords)obj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        void DeleteProfileCoord(object obj) {
            if(SelectedProfileCoords == null) return;
            db.ProfileCoords.Remove(SelectedProfileCoords);
            db.SaveChanges();
        }

        void AddPicket(object obj) {
            var coord = new Picket() { X = 0, Y = 0, Profile = Profile };
            db.Pickets.Add(coord);
            db.SaveChanges();
            SelectedPicket = coord;
            OnPropertyChanged(nameof(Profile));
        }

        void SavePicketCoord(object obj) {
            if(obj is Picket) {
                db.Entry((Picket)obj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        void DeletePicket(object obj) {
            if(SelectedPicket == null) return;
            db.Pickets.Remove(SelectedPicket);
            db.SaveChanges();
        }

        void OpenPicket(object obj) {
            if(SelectedOperator == null) return;    
            new PicketWindow() {
                DataContext = new PicketVM((Picket)obj, SelectedOperator)
            }.ShowDialog();
            OnPropertyChanged(nameof(obj));
        }

        private DrawingImage image;
        public DrawingImage Image {
            get => image;
            set { image = value; OnPropertyChanged(nameof(Image)); }
        }
        void Redraw() {
            var newimage = new DrawModel();
            if(Profile.IsCorrect()) Profile.Draw(newimage, Brushes.Black);
            else Profile.Draw(newimage, Brushes.Red);
            foreach(var p in Profile?.Coords ?? new()) {
                if(p == SelectedProfileCoords) newimage.DrawCircle(p.X, p.Y, 0.15, Brushes.Green);
                else newimage.DrawCircle(p.X, p.Y, 0.15, Brushes.DarkGray);
            }
            foreach(var p in Profile?.Pickets ?? new()) {
                if(p == SelectedPicket && p.IsOnProfile()) newimage.DrawFlag(p.X, p.Y, 0.17, Brushes.Red);
                else if(p == SelectedPicket && !p.IsOnProfile()) newimage.DrawFlag(p.X, p.Y, 0.17, Brushes.Black);
                else if(p != SelectedPicket && p.IsOnProfile()) newimage.DrawFlag(p.X, p.Y, 0.17, Brushes.DarkRed);
                else if(p != SelectedPicket && !p.IsOnProfile()) newimage.DrawFlag(p.X, p.Y, 0.17, Brushes.DarkOliveGreen);
            }
            Image = newimage.Render();
        }
    }
}
