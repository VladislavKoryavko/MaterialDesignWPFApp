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
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Settings;

namespace DidaktikaApp.ViewModel.EducationalEquipment.Oscilloscope
{
    public class OscilloscopeViewModel : BaseViewModel
    {
        private OscilloscopeModel _modelOscilloscope = new OscilloscopeModel();
        public OscilloscopeModel ModelOscilloscope { get => _modelOscilloscope; set => _modelOscilloscope = value; }

        public OscilloscopeViewModel()
        {
            //forward properties from the model to view model           
            ModelOscilloscope.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            #region Delegate commands           
            SetDefoultSettings = new DelegateCommand(_modelOscilloscope.SetDefoultSettings);
            StartConnection = new DelegateCommand(_modelOscilloscope.StartConnection);
            StopConnection = new DelegateCommand(_modelOscilloscope.StopConnection);
            #endregion

            XVal = 50;
            YVal = 50;

        }

        #region Text items for ComboBoxes


        //public List<String> Triggers => _modelOscilloscope.TriggerStates;
        public List<String> TypeChanelCurent => _modelOscilloscope.TypeChanelCurent;
        #endregion


 
        public DelegateCommand SetDefoultSettings { get; }
        public DelegateCommand StartConnection { get; }
        public DelegateCommand StopConnection { get; }



        public void OnMouse()
        {

        }

        public int XVal { get; set; }
        public int YVal { get; set; }
    }
}
