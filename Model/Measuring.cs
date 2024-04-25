using System.Collections.ObjectModel;

namespace AgeenkovApp.Model {
    public class Measuring : PropChange {
        private int id;
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private Operator oper;
        public Operator Operator {
            get => oper;
            set { oper = value; OnPropertyChanged(nameof(Operator)); }
        }

        private Picket picket;
        public Picket Picket {
            get => picket;
            set { picket = value; OnPropertyChanged(nameof(Picket)); }
        }

        private DateTime measuringDateTime;
        public DateTime MeasuringDateTime {
            get => measuringDateTime;
            set { measuringDateTime = value; OnPropertyChanged(nameof(MeasuringDateTime)); }
        }

        private double measuringValue;
        public double MeasuringValue {
            get => measuringValue;
            set { measuringValue = value; OnPropertyChanged(nameof(MeasuringValue)); }
        }
    }
}
