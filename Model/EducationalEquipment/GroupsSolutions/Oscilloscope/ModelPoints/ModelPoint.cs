using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelPoints
{
    public class ModelPoint : INotifyPropertyChanged //: Model
    {
        #region Property X(float)

        private float _x;

        public ModelPoint(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public float X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        #endregion

        #region Property Y(float)

        private float _y;

        public float Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        #endregion

        #region
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
