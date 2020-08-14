using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Settings
{
    [Serializable]
    public class SettingsConnections
    {
        public SettingsConnections()
        {
        }

        public ObservableCollection<String> BitsToSend { get; set; }
        public int CountBits { get; set; }
        public int LengthPackage { get; set; }
        public uint BaudRate { get; set; }
        public byte DataBits { get; set; }
        public byte StopBits { get; set; }
        public byte Parity { get; set; }

        
        
    }
}
