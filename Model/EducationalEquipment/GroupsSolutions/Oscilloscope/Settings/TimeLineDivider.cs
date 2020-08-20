using System;
using System.Collections.Generic;
using System.Text;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Settings
{
    public class TimeLineDivider
    {
        private byte forSend;
        private string forView;
        private float coefficientDivider;
        private string axisTitle;
        private float axisMinValue;
        private float axisMaxValue;
        private float axisStep;
        private string group;


        public TimeLineDivider(byte forSend, string forView, float coefficientDivider, string axisTitle, float axisMinValue, float axisMaxValue, float axisStep, string group)
        {
            ForSend = forSend;
            ForView = forView;
            CoefficientDivider = coefficientDivider;
            AxisTitle = axisTitle;
            AxisMinValue = axisMinValue;
            AxisMaxValue = axisMaxValue;
            AxisStep = axisStep;
            Group = group;
        }

        public TimeLineDivider(byte forSend, string forView, double coefficientDivider, string axisTitle, double axisMinValue, double axisMaxValue, double axisStep, string group)
        {
            ForSend = forSend;
            ForView = forView;
            CoefficientDivider = (float)coefficientDivider;
            AxisTitle = axisTitle;
            AxisMinValue = (float)axisMinValue;
            AxisMaxValue = (float)axisMaxValue;
            AxisStep = (float)axisStep;
            Group = group;
        }

        public byte ForSend { get => forSend; set => forSend = value; }
        public string ForView { get => forView; set => forView = value; }
        public float CoefficientDivider { get => coefficientDivider; set => coefficientDivider = value; }
        public string AxisTitle { get => axisTitle; set => axisTitle = value; }
        public float AxisMinValue { get => axisMinValue; set => axisMinValue = value; }
        public float AxisMaxValue { get => axisMaxValue; set => axisMaxValue = value; }
        public float AxisStep { get => axisStep; set => axisStep = value; }
        public string Group { get => group; set => group = value; }
    }
}
