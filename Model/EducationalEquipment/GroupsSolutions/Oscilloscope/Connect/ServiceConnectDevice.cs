using System;
using System.Collections.Generic;
using System.Text;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Connect
{
    public static class ServiceConnectDevice
    {
        //private BlockRead block = new BlockRead();
        private static ConnectDevice connectDevice = new ConnectDevice();

        static ServiceConnectDevice()
        {
        }

        public static IEnumerable<byte[]> ReadValue(int lengthBits, byte[] bitsToSend, uint speed)
        {
            foreach (var readData in connectDevice.ReadDatas(lengthBits, bitsToSend, speed)) yield return readData;
        }
    }
}
