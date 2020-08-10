using Didaktika.PD.FTDI;
using FTD2XX_NET;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Connect
{
    public class FTDIPort
    {

        public FTDI ftdiDevice { get; }

        public Action<string> ErrorOccured { get; set; }

        public FTDIPort(FTDI ftdiDevice)
        {
            this.ftdiDevice = ftdiDevice;
        }

        public bool Open()
        {
            return ftdiDevice.OpenByIndex(0) == FTDI.FT_STATUS.FT_OK;
        }

        public bool Close()
        {
            return this.ftdiDevice.Close() == FTDI.FT_STATUS.FT_OK;
        }

        public void SetTimeouts(uint read, uint write)
        {
            int num = (int)this.ftdiDevice.SetTimeouts(read, write);
        }

        public void SetConnectionParams(uint baud, byte size, StopBits stopBits, Parity parity)
        {
            //ftdiDevice.SetBaudRate()
            int num1 = (int)this.ftdiDevice.SetBaudRate(baud);
            int num2 = (int)this.ftdiDevice.SetDataCharacteristics(size, this.ConvertStopBits(stopBits),
                this.ConvertParity(parity));
        }

        public bool Write(byte[] data)
        {
            uint numBytesWritten = 0;
            return this.ftdiDevice.Write(data, data.Length, ref numBytesWritten) == FTDI.FT_STATUS.FT_OK &&
                   (long)numBytesWritten == (long)data.Length;
        }

        public bool Write(byte data)
        {
            uint numBytesWritten = 0;
            return this.ftdiDevice.Write(new byte[] { data }, 1, ref numBytesWritten) == FTDI.FT_STATUS.FT_OK &&
                   numBytesWritten == 1;
        }

        public void Purge()
        {
            ftdiDevice.Close();
            ftdiDevice.Purge(FTDI.FT_PURGE.FT_PURGE_RX);
            ftdiDevice.Purge(FTDI.FT_PURGE.FT_PURGE_TX);
        }

        public bool Read(byte[] data, int start, int count)
        {
            uint numBytesRead = 0;
            var k = this.ftdiDevice.Read(data, (uint)data.Length, ref numBytesRead) == FTDI.FT_STATUS.FT_OK &&
                    (long)numBytesRead == (long)data.Length;
            return k;
        }

        public void EmptyQueue()
        {
            int num = (int)this.ftdiDevice.Purge(3U);
        }

        private byte ConvertStopBits(StopBits stopBits)
        {
            switch (stopBits)
            {
                case StopBits.One:
                    return 0;
                case StopBits.Two:
                    return 2;
                default:
                    throw new Exception("unknown stopbits");
            }
        }

        private byte ConvertParity(Parity parity)
        {
            switch (parity)
            {
                case Parity.None:
                    return 0;
                case Parity.Odd:
                    return 1;
                case Parity.Even:
                    return 2;
                case Parity.Mark:
                    return 3;
                case Parity.Space:
                    return 4;
                default:
                    throw new Exception("unknown parity");
            }


        }
        #region MyRegion
        public EventWaitHandle Handle1 = new EventWaitHandle(false, EventResetMode.AutoReset);
        public EventWaitHandle Handle2 = new AutoResetEvent(true);
        public EventWaitHandle Handle3 = new AutoResetEvent(true);

        public bool SetEvent()
        {
            // ftdiDevice.getR
            var k1 = ftdiDevice.SetEventNotification(FTDI.FT_EVENTS.FT_EVENT_RXCHAR, Handle1);
            var k2 = ftdiDevice.SetEventNotification(FTDI.FT_EVENTS.FT_EVENT_MODEM_STATUS, Handle2);
            var k3 = ftdiDevice.SetEventNotification(FTDI.FT_EVENTS.FT_EVENT_LINE_STATUS, Handle3);
            if ((k1 == k2) && (k1 == k3) && k1 == FTDI.FT_STATUS.FT_OK) return true;
            return false;
        }


        #endregion
    }
}
