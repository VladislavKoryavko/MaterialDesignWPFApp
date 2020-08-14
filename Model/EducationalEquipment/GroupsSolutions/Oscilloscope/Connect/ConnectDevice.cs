using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Didaktika.PD.FTDI;
using FTD2XX_NET;

namespace DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Connect
{
    public class ConnectDevice
    {
        //дополнительные системные байты к принимаемой посылке ВСУММЕ
        public const byte SummAddtionBytes = 3;

        //дополнительные системные байты к принимаемой посылке ВНАЧАЛЕ
        public const byte StartAddtionBytes = 1;

        //Описание устройства
        public string Description;

        //Адрес с которого начинаем чтение
        private ushort Address;

        //регистр в котором происходит чтение и запись триггера
        static ushort Reg_Start;

        //пакет который будет отправляться для получения данных
        public byte[] PacketBytesToSend
        {
            get => _packetBytesToSend;
            set => _packetBytesToSend = value;
        }

        static IFTDISearcher seacher = new FTDISearcher();
        static IFTDIConnection _dataPD;
        private static FTDI _ftdi { get; set; }
        private static FTDIPort _ftdiPort;

        static int PDIndex = 0;


        // коэффициент получаемый из первой ячейки массива полученных данных
        public static byte Coefficient;

        //Хранение полученных данных перемноженных на первый элемент(коэффициент)
        public static IEnumerable<int> PaсketInt; // = new byte[8*1024];
        public static event EventHandler<IEnumerable<int>> ChangePaсket;

        public ConnectDevice()
        {
            _ftdi = new FTDI();
            _ftdiPort = new FTDIPort(_ftdi);
            _ftdi.OpenByIndex(0);
            var f = new FTDI.FT_DEVICE_INFO_NODE[10];
            _ftdi.GetDeviceList(f);
            _ftdiPort.SetConnectionParams(256000, 8, StopBits.Two, Parity.Space);
            //uint coef = 100 / (3967 - 20);
            _ftdiPort.SetTimeouts(2000, 800);

            StartListenDevice();
        }

        public long PingMillisecond
        {
            get
            {
                var f = PingWatch.ElapsedMilliseconds;
                PingWatch.Reset();
                return f;
            }
        }

        public bool TurnRound(byte bit)
        {
            return _ftdiPort.Write(bit);
        }

        private Stopwatch PingWatch = new Stopwatch();
        private byte[] _packetBytesToSend = new byte[5] { 0x00, 0x0f, 0x01, 0x01, 0x83 };

        public void StartListenDevice()
        {
            // new Thread(
            //     ReadDatas
            // ).Start();
        }

        private uint lastSpeed = 0;

        public IEnumerable<byte[]> ReadDatas(int lengthBits, byte[] bitsToSend, uint speed)
        {
            if (lastSpeed != speed)
            {
                _ftdi.SetBaudRate(speed);
                lastSpeed = speed;
            }

            while (true)
            {
                var countSend = 0;
                byte[] Paсket = new byte[lengthBits];
                
                //PingWatch.Start();
                try
                {
                    //Console.WriteLine("set=" + string.Join(", ", bitsToSend));
                    foreach (var bit8 in bitsToSend)
                    {
                        //Thread.Sleep(1);
                        countSend++;
                        if (!_ftdiPort.Write(bit8))
                        {
                            _ftdi = new FTDI();
                            _ftdiPort = new FTDIPort(_ftdi);
                        }
                    }

                    _ftdiPort.Read(Paсket, 0, lengthBits);
                    //Console.WriteLine("get=" + string.Join(", ", Paсket));
                }
                catch (Exception exception)
                {
                    //Console.WriteLine(exception);
                    //  for (int i = 0; i < countSend; i++)
                    //  {
                    //     // _ftdiPort.Write(0);
                    //      _ftdiPort.Read(new byte[1],0,1 );
                    //  }
                }

                //PingWatch.Stop();
                yield return Paсket;
            }
        }

        public static bool SetEvent()
        {
            //_ftdiPort.Handle1.
            return _ftdiPort.SetEvent();
        }
    }
}
