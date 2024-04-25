using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

namespace AgeenkovApp.Model {
    public class AreaCoords : PropChange {
        private int id;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {
            get { return id; }
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

        private Area area;
        public Area Area {
            get => area;
            set { area = value; OnPropertyChanged(nameof(Area)); }
        }

        public Point AsPoint => new Point(x, y);
    }
}
