using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelPoints;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelGraphic
{
    public class ModelGraphic : INotifyPropertyChanged //: Model
    {
        #region Property ListValuesOnGraphic(ObservableCollection<ModelPoint>)

        private ObservableCollection<ModelPoint> _listValuesOnGraphic;

        public ObservableCollection<ModelPoint> ListValuesOnGraphic
        {
            get { return _listValuesOnGraphic; }
            set
            {
                _listValuesOnGraphic = value;
                OnPropertyChanged(nameof(ListValuesOnGraphic));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
