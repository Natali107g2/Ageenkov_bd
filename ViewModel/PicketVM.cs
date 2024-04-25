using AgeenkovApp.Model;
using AgeenkovApp.View;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using System.Windows;

namespace AgeenkovApp.ViewModel {
    class PicketVM : PropChange {
        AggContext db = AggContext.LoadAll();

        public Picket Picket { get; set; }
        public Operator Operator { get; set; }
        public ObservableCollection<Measuring> Measurings { get; set; }

        public RelayCommand AddMeasuringCmd { get; set; }
        public RelayCommand DeleteMeasuringCmd { get; set; }
        public RelayCommand RefreshCmd { get; set; }

        public PicketVM(Picket picket, Operator oper) {
            Picket = picket;
            Operator = oper;

            Measurings = db.Measurings.Local.ToObservableCollection();

            AddMeasuringCmd = new(AddMeasuring);
            DeleteMeasuringCmd = new(DeleteMeasuring);
            RefreshCmd = new(Refresh);
        }

        void AddMeasuring(object obj) {
            var meas = new Measuring() { MeasuringValue = 0 };
            if(new NewMeasuring(meas).ShowDialog() == false) return;
            //else if(meas.MeasuringValue == null) { return; } 
            else {
                meas.Operator = Operator;
                meas.Picket = Picket;
                meas.MeasuringDateTime = DateTime.Now;
                db.Measurings.Add(meas);
                db.SaveChanges();
                OnPropertyChanged(nameof(Picket));
                SetPlotModel();
            }
        }

        void DeleteMeasuring(object obj) {
            if(MessageBox.Show("Are you sure?", "Delete measuring",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                db.Measurings.Remove((Measuring)obj);
                db.SaveChanges();
                SetPlotModel();
            }
        }

        void Refresh(object obj) {
            SetPlotModel();
        }

        private PlotModel plotModel;
        public PlotModel PlotModel {
            get => plotModel;
            set { plotModel = value; OnPropertyChanged(nameof(PlotModel)); }
        }
        private void SetPlotModel() {
            var plotModel = new PlotModel { Title = "График значений" };

            var xAxis = new DateTimeAxis {
                Position = AxisPosition.Bottom,
                Title = "Время измерения",
                StringFormat = "MM/dd/yyyy HH:mm", // Customize the date and time format if needed
                IntervalType = DateTimeIntervalType.Seconds,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None
            };

            var yAxis = new LinearAxis {
                Position = AxisPosition.Left,
                Title = "Значение измерения",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None
            };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            foreach(var meas in Measurings) {
                if(meas.MeasuringValue != null) {
                    var lineSeries = new LineSeries {
                        Title = "График",
                        Color = OxyColors.Black,
                        StrokeThickness = 3,
                        MarkerType = MarkerType.Circle,
                        MarkerSize = 2,
                        MarkerStroke = OxyColors.Red,
                        MarkerFill = OxyColors.Red
                    };
                    foreach(var val in Picket.Measurings) {
                        lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(val.MeasuringDateTime), val.MeasuringValue));
                    }
                    plotModel.Series.Add(lineSeries);
                }
            }
            PlotModel = plotModel;
        }
    }
}
