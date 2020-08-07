using DidaktikaApp.Model.EducationalEquipment.Oscilloscope;
using DidaktikaApplication.Helpers;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;

namespace DidaktikaApp.ViewModel.EducationalEquipment.Oscilloscope
{
    public class OscilloscopeViewModel : BaseViewModel
    {
        OscilloscopeModel _modelOscilloscope = new OscilloscopeModel();

        public OscilloscopeViewModel()
        {
            //forward properties from the model to view model
            
            _modelOscilloscope.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            Cartesian();
            timerStart();
            //GoToSolution = new DelegateCommand<string>(str => { _modelApp.GoToSolution(str); });
            //_modelOscilloscope.ChangedSolution += OpenSelectedView;
        }


        public void Cartesian()
        {
            _modelOscilloscope.SeriesCollectionPoints = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Chanel A", 
                    Values=new ChartValues<int>{1,50,100},
                    //Values=new int[1000],
                    PointGeometry = null,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0.1
                },
                new LineSeries
                {
                    Title = "Chanel B", Values=new ChartValues<int>(),
                    PointGeometry = null,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0.3
                }
            };
            Labels = new[] { "Триггер",};
            //yFormatter = value => value.ToString("C");

            for (int i = 0; i < OscilloscopeModel.maxPointInChart; i++)
            {
                _modelOscilloscope.SeriesCollectionPoints[1].Values.Add(0);
            }
            
        }
        public List<String> VoltageScaleDividers => _modelOscilloscope.TimeLineDivisions;
        public List<String> Triggers => _modelOscilloscope.Triggers;

        public Func<double, string> yFormatter { get; set; }

        public SeriesCollection SeriesCollectionPoints => _modelOscilloscope.SeriesCollectionPoints;
        public string[] Labels { get; set; }

        

        private DispatcherTimer timer = null;
        private int NumberTimerTick = 1;

        private void timerStart()
        {
            timer = new DispatcherTimer();  // если надо, то в скобках указываем приоритет, например DispatcherPriority.Render
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            _modelOscilloscope.SetRandomPoints(1);
        }

    }
}
