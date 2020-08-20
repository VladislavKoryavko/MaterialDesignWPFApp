using System;
using System.Collections.Generic;
using System.Text;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Settings
{
    public class ACDCSettings
    {
        private byte forSend;
        private string forView;

        public ACDCSettings(byte forSend, string forView)
        {
            ForSend = forSend;
            ForView = forView;
        }

        public byte ForSend { get => forSend; set => forSend = value; }
        public string ForView { get => forView; set => forView = value; }
    }
}
