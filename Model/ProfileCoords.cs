using System.Windows;

namespace AgeenkovApp.Model {
    public class ProfileCoords : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private double x;
        public double X {
            get => x;
            set { x = value; OnPropertyChanged(nameof(X)); }
        }

        private double y;
        public double Y {
            get => y;
            set { y = value; OnPropertyChanged(nameof(Y)); }
        }

        private Profile profile;
        public Profile Profile {
            get => profile;
            set { profile = value; OnPropertyChanged(nameof(Profile)); }
        }

        public Point AsPoint => new Point(x, y);
    }
}
