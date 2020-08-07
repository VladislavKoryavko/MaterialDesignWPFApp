using LiveCharts;
using Prism.Mvvm;
using System;
using System.Collections.Generic;


namespace DidaktikaApp.Model.EducationalEquipment.Oscilloscope
{
    public class OscilloscopeModel : BindableBase
    {
        public List<string> TimeLineDivisions = new List<string> { "200 mV", "450 mV", "1 V", "2 V", "4.5 V", "10 V", "20 V", "45 V", "100 V", "200 V" };
        public List<string> Triggers = new List<string> { "Нет", "Авто", "Канал А", "Канал В"};
        //Dictionary<string,string> TimeLineDivisions = new Dictionary<int, string> { { "200", "mV"}, { "450", "mV" }, { "200", "V" }, { "200", "V" }, { "200", "V" }, { "200", "V" }, { "200", "V" }, { "200", "V" }, }

        public static int maxPointInChart = 500;
        private int STEP = 1;
        private int FrameInStap = 250;

        private Random random = new Random();
        object[] mass = new object[250];

        

        private SeriesCollection _seriesCollectionPoints;


        public SeriesCollection SeriesCollectionPoints;

        public OscilloscopeModel()
        {
            for (int i = 0; i < FrameInStap; i++)
            {
                mass[i] = 0;
            }
        }

        //{ 
        //    get => _seriesCollectionPoints;
        //    set
        //    {
        //        _seriesCollectionPoints = value;
        //        //RaisePropertyChanged("SeriesCollectionPoints");
        //    }
        //}

        public void SetRandomPoints(int chanel)
        {


            //SeriesCollectionPoints[chanel].Values[nuberTikc] = random.Next(0, 255);
            //RaisePropertyChanged("SeriesCollectionPoints");

            if(STEP>=maxPointInChart/FrameInStap)
            {
                STEP = 0;
                //SeriesCollectionPoints[chanel].Values.Clear();
            }

            for (int i = 0; i < FrameInStap; i++)
            {
                mass[i] = random.Next(0, 255);
                SeriesCollectionPoints[chanel].Values.Insert(i + STEP* FrameInStap, mass[i]);
                SeriesCollectionPoints[chanel].Values.RemoveAt(maxPointInChart);
            }

            STEP++;
            
            
            //SeriesCollectionPoints[chanel].Values.Clear();
            //SeriesCollectionPoints[chanel].Values.AddRange(mass);

            //SeriesCollectionPoints[chanel].Values.Clear();
            //SeriesCollectionPoints[chanel].Values.AddRange(mass.ToList());


            
            //for (int i = 0; i < maxPointInChart; i++)
            //{
            //    //SeriesCollectionPoints[chanel].Values.Add(random.Next(0, 255));
            //    SeriesCollectionPoints[chanel].Values.AddRange(mass);
            //}
            
        }
    }


}
