using DidaktikaApp.Helpers.Utils.Serelization;
using DidaktikaApp.Model.EducationalEquipment.Oscilloscope;
using DidaktikaApplication.Helpers;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using System.Linq;
using System.Globalization;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Connect;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelGraphic;
using System.Xml.Serialization;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelPoints;
using Prism.Commands;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace DidaktikaApp.ViewModel.EducationalEquipment.Oscilloscope
{
    public class OscilloscopeViewModel : BaseViewModel
    {
        OscilloscopeModel _modelOscilloscope = new OscilloscopeModel();

        public OscilloscopeViewModel()
        {
            //forward properties from the model to view model
            
            _modelOscilloscope.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            //Cartesian();
            //timerStart();
            //GoToSolution = new DelegateCommand<string>(str => { _modelApp.GoToSolution(str); });
            //_modelOscilloscope.ChangedSolution += OpenSelectedView;

            StartConnection = new DelegateCommand<ObservableCollection<string>>(bitForSend => { StartConnections(bitForSend); });

            #region Connect settings

            CountBits =  20;
            LenghtPackage =  1007;
            Speed = 921600;
            #endregion
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




        #region Connection

        private Thread ReadData;

        public class Data
        {
            public ObservableCollection<String> BitsToSend { get; set; }
            public int CountBits { get; set; }
            public int LengthPackage { get; set; }
            public uint Speed { get; set; }
        }


        public DelegateCommand<ObservableCollection<string>> StartConnection { get; }
        public void StartConnections(ObservableCollection<string> bitForSend)
        {
            //For test
            BitsToSend = new ObservableCollection<string>()
            {
                "AA", "FF" , "FF", "00", "00", "cf", "83", "7f", "20", "f4", "01" , "7f", "7f", "7f", "25", "c3", "66" , "6f", "00", "01"
            };

            var ArrayBytesToSend = BitsToSend.Select(s => byte.Parse(s, NumberStyles.AllowHexSpecifier)).ToArray();
            

            //Console.WriteLine(string.Join(", ", ArrayBytesToSend));
            ReadData?.Abort();
            ReadData = new Thread(() =>
            {
                foreach (var list in ServiceConnectDevice.ReadValue(LenghtPackage, ArrayBytesToSend, Speed))
                {
                    int count = 0;
                    var k = list.Select(b => new ModelPoint(count++, b)).ToList();
                    Graphic = new ModelGraphic()
                    {
                        ListValuesOnGraphic = new ObservableCollection<ModelPoint>(k)
                    };
                }
            })
            { IsBackground = true };
            ReadData.Start();
        }


        #region Property BitsToSend(List<string>)

        private ObservableCollection<string> _BitsToSend = new ObservableCollection<string>();

        public ObservableCollection<string> BitsToSend
        {
            get { return _BitsToSend; }
            set
            {
                _BitsToSend = value;
                SerializedThis();
                OnPropertyChanged(nameof(BitsToSend));
            }
        }

        #endregion

        #region Property CountBits(int)

        private int _countBits;

        public int CountBits
        {
            get { return _countBits; }
            set
            {
                _countBits = value;
                SerializedThis();
                OnPropertyChanged(nameof(CountBits));
            }
        }

        #endregion

        #region Property LenghtPackage(int)

        private int _lenghtPackage;

        public int LenghtPackage
        {
            get { return _lenghtPackage; }
            set
            {
                _lenghtPackage = value;
                SerializedThis();
                OnPropertyChanged(nameof(LenghtPackage));
            }
        }

        #endregion

        #region Property Speed(int)

        private uint _speed;

        public uint Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                SerializedThis();
                OnPropertyChanged(nameof(Speed));
            }
        }

        #endregion

        #region Property Graphic(ModelGraphic)

        private ModelGraphic _graphic;

        [XmlIgnore]
        public ModelGraphic Graphic
        {
            get { return _graphic; }
            set
            {
                _graphic = value;
                OnPropertyChanged(nameof(Graphic));
            }
        }

        #endregion

        private void SerializedThis() =>
            UtilSerialized.Serializable(
                new Data()
                {
                    BitsToSend = BitsToSend,
                    CountBits = CountBits,
                    LengthPackage = LenghtPackage,
                    Speed = Speed
                },
                Environment.CurrentDirectory + "\\Data.xml");
    }
    #endregion
}
