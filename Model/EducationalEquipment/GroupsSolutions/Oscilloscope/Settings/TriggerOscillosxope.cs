using System;
using System.Collections.Generic;
using System.Text;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Settings
{
    public class TriggerOscillosxope
    {
        private byte forSend;
        private string forView;

        public TriggerOscillosxope(byte forSend, string forView)
        {
            ForSend = forSend;
            ForView = forView;
        }

        public byte ForSend { get => forSend; set => forSend = value; }
        public string ForView { get => forView; set => forView = value; }
    }
}
