using System.Collections.ObjectModel;
using System.Windows;

namespace AgeenkovApp.Model {
    public class Picket : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private Profile profile;
        public Profile Profile {
            get => profile;
            set { profile = value; OnPropertyChanged(nameof(Profile)); }
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


        private ObservableCollection<Measuring> measurings;
        public ObservableCollection<Measuring> Measurings {
            get => measurings;
            set { measurings = value; OnPropertyChanged(nameof(Measurings)); }
        }
        public Point AsPoint => new Point(x, y);

        public bool IsOnProfile() {
            foreach(var point in profile.Coords) {
                if(X == point.X && Y == point.Y) return true;
            }
            foreach(var lineSegment in GetLineSegments()) {
                if(ArePointsOnLineSegment(X, Y, lineSegment.Item1.X, lineSegment.Item1.Y, lineSegment.Item2.X, lineSegment.Item2.Y)) return true;
            }
            return false;
        }

        private ObservableCollection<Tuple<Point, Point>> GetLineSegments() {
            var lineSegments = new ObservableCollection<Tuple<Point, Point>>();

            for(int i = 0; i < profile.Coords.Count - 1; i++) {
                lineSegments.Add(new Tuple<Point, Point>(profile.Coords[i].AsPoint, profile.Coords[i + 1].AsPoint));
            }

            return lineSegments;
        }
        private bool ArePointsOnLineSegment(double px, double py, double x1, double y1, double x2, double y2) {
            double d1 = Distance(px, py, x1, y1);
            double d2 = Distance(px, py, x2, y2);
            double lineLength = Distance(x1, y1, x2, y2);

            return Math.Abs(d1 + d2 - lineLength) < 0.001;
        }

        private double Distance(double x1, double y1, double x2, double y2) {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
