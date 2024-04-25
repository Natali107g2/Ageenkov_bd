using AgeenkovApp.Model;
using AgeenkovApp.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AgeenkovApp.ViewModel {
    public class AreaVM : PropChange {
        AggContext db = AggContext.LoadAll();

        public ObservableCollection<Profile> Profiles { get; set; }
        public ObservableCollection<AreaCoords> AreaCoords { get; set; }

        public RelayCommand AddCoordCmd { get; set; }
        public RelayCommand DeleteCoordCmd { get; set; }
        public RelayCommand AddProfileCmd { get; set; }
        public RelayCommand DeleteProfileCmd { get; set; }
        public RelayCommand OpenProfileCmd { get; set; }
        public RelayCommand SaveCoordCmd { get; set; }

        public Area Area { get; set; }
        public AreaVM(Area area) {
            Area = area;

            Profiles = db.Profiles.Local.ToObservableCollection();
            AreaCoords = db.AreaCoords.Local.ToObservableCollection();

            AddCoordCmd = new(AddCoord);
            DeleteCoordCmd = new(DeleteCoord);
            AddProfileCmd = new(AddProfile);
            DeleteProfileCmd = new(DeleteProfile);
            OpenProfileCmd = new(OpenProfile);
            SaveCoordCmd = new(SaveCoord);
        }

        public string AreaName {
            get => Area.Name;
            set {
                Area.Name = value;
                db.Entry(Area).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private AreaCoords selectedCoord;
        public AreaCoords SelectedCoord {
            get => selectedCoord;
            set { selectedCoord = value; OnPropertyChanged(nameof(SelectedCoord)); Redraw(); }
        }

        private Profile selectedProfile;
        public Profile SelectedProfile {
            get => selectedProfile;
            set { selectedProfile = value; OnPropertyChanged(nameof(SelectedProfile)); Redraw(); }
        }

        void AddCoord(object obj) {
            var coord = new AreaCoords() { X = 0, Y = 0, Area = Area};
            db.AreaCoords.Add(coord);
            db.SaveChanges();
            SelectedCoord = coord;
            OnPropertyChanged(nameof(Area));
        }

        void DeleteCoord(object obj) {
            if(SelectedCoord == null) return;
            if(MessageBox.Show("Are you sure?", "Delete coord",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes){
                db.AreaCoords.Remove(selectedCoord);
                db.SaveChanges();
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Area.Coords));
            }
        }

        void SaveCoord(object obj) {
            if (obj is AreaCoords) {
                db.Entry((AreaCoords)obj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        void AddProfile(object obj) {
            var profile = new Profile() { Area = Area };
            db.Profiles.Add(profile);
            db.SaveChanges();
            selectedProfile = profile;
            OnPropertyChanged(nameof(Area));
        }

        void DeleteProfile(object obj) {
            if(SelectedProfile == null) return;
            db.Profiles.Remove(SelectedProfile);
            db.SaveChanges();
            OnPropertyChanged(nameof(Area));
        }

        void OpenProfile(object obj) {
            new ProfileWindow() {
                DataContext = new ProfileVM((Profile)obj)
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
            if(Area.IsCorrect()) Area.Draw(newimage, Brushes.Black);
            else Area.Draw(newimage, Brushes.Red);
            foreach(var point in Area?.Coords ?? new()) {
                if(SelectedCoord == point && Area.IsCorrect()) newimage.DrawCircle(point.X, point.Y, 0.4, Brushes.Yellow);
                else if(SelectedCoord == point && !Area.IsCorrect()) newimage.DrawCircle(point.X, point.Y, 0.4, Brushes.Red);
                else if(SelectedCoord != point && Area.IsCorrect()) newimage.DrawCircle(point.X, point.Y, 0.4, Brushes.Black);
                else if(SelectedCoord != point && !Area.IsCorrect()) newimage.DrawCircle(point.X, point.Y, 0.4, Brushes.IndianRed);
            }
            foreach(var profile in Area?.Profiles ?? new()) {
                if(SelectedProfile == profile && profile.IsCorrect()) profile.Draw(newimage, Brushes.Green);
                else if(SelectedProfile == profile && !profile.IsCorrect()) profile.Draw(newimage, Brushes.Red);
                else if(SelectedProfile != profile && profile.IsCorrect()) profile.Draw(newimage, Brushes.DarkBlue);
                else if(SelectedProfile != profile && !profile.IsCorrect()) profile.Draw(newimage, Brushes.IndianRed);
            }
            Image = newimage.Render();
        }
    }
}
