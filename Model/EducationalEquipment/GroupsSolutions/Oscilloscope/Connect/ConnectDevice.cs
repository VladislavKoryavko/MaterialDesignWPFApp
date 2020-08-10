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

        //private static void Setings()
        //{
        //    _ftdiPort.SetConnectionParams(115200, 8, StopBits.Two, Parity.Space);
        //    _ftdiPort.SetTimeouts(UInt32.MaxValue, UInt32.MaxValue);
        //}
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
                //for (int i = 0; i < lengthBits; i++)
                //{
                //    _ftdiPort.Write(0);
                //}
                PingWatch.Start();
                try
                {
                    Console.WriteLine("set=" + string.Join(", ", bitsToSend));
                    foreach (var bit8 in bitsToSend)
                    {
                        Thread.Sleep(1);
                        countSend++;
                        if (!_ftdiPort.Write(bit8))
                        {
                            _ftdi = new FTDI();
                            _ftdiPort = new FTDIPort(_ftdi);
                        }
                    }

                    //Thread.Sleep(new TimeSpan(1L));
                    _ftdiPort.Read(Paсket, 0, lengthBits);
                    Console.WriteLine("get=" + string.Join(", ", Paсket));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    //  for (int i = 0; i < countSend; i++)
                    //  {
                    //     // _ftdiPort.Write(0);
                    //      _ftdiPort.Read(new byte[1],0,1 );
                    //  }
                }

                PingWatch.Stop();
                yield return Paсket;
            }
        }

        public static bool SetEvent()
        {
            //_ftdiPort.Handle1.
            return _ftdiPort.SetEvent();
        }
        //byte[] packet = new byte[8 * 1024];

        //private void ReaderDevice()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            if (MainWindow.IsWorkProgramm) break;
        //            Thread.Sleep(50);
        //            OpenDevice();
        //            byte[] packet = new byte[8 * 1024];
        //            _ftdiPort.Write(0b11101);
        //            for (int i = 0; i < packet.Length; i++)
        //            {
        //                _ftdiPort.Read(packet, i, 1);
        //            }

        //            ushort x = 0;
        //            ListPoinst = new List<Point>(packet.Select(y =>
        //            {
        //                x++;
        //                return new Point(y, x);
        //            }));

        //            OnPropertyChanged(nameof(ListPoinst));
        //            CountRead++;
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception);
        //        if (!MainWindow.IsWorkProgramm)
        //            StartListenDevice();
        //    }
        //}


        /*   private void CatchDevice()
           {
               //WaitDevice();

               /*
               for (int i = 0; i < ListDeviceInfo.Count; i++)
               {
                   if (ListDeviceInfo[i].Description.Contains(Description))
                       PDIndex = i;
               }

               if (_dataPD == null)
               {
                   _dataPD = seacher.OpenDevice(ListDeviceInfo[PDIndex], 19200, DataBits.Bits8, StopBits.StopBits1,
                       Parity.ParityNone);

                   _dataPD.DataHelper.WriteUShort(1, Reg_Start, 1);
               }

               new Thread(ReaderDevice).Start();
               //DeviceInfo[] dInfo = seacher.GetAllDevices().ToArra
           }
            */

        //int temp;
        //private long _countRead = 0;

        //private void WaitDevice()
        //{
        //    //TextLoadDevice = "Подключение"
        //    // CountDevice = 0;

        //    while (seacher.DeviceCount < 1)
        //    {

        //        temp++;
        //        /* if (temp % 2720 == 0)
        //         {
        //             switch (TextLoadDevice)
        //             {
        //                 case "Подключение":
        //                     TextLoadDevice = "Подключение.";
        //                     break;
        //                 case "Подключение.":
        //                     TextLoadDevice = "Подключение..";
        //                     break;
        //                 case "Подключение..":
        //                     TextLoadDevice = "Подключение...";
        //                     break;
        //                 case "Подключение...":
        //                     TextLoadDevice = "Подключение";
        //                     break;
        //                 default:
        //                     TextLoadDevice = "Подключение";
        //                     break;
        //             }
        //         }
        //         */

        //        if (temp == 100000) temp = 0;
        //    }

        //    //TextLoadDevice = "Подключено";
        //    //CountDevice = (int)seacher.DeviceCount;
        //    //ImageSource = IsConnect;
        //    ListDeviceInfo = seacher.GetAllDevices().ToList();
        //}

        /*    public List<Point> ListPoinst { get; set; }
    
            public long CountRead
            {
                get => _countRead;
                set
                {
                    _countRead = value;
                    OnPropertyChanged(nameof(CountRead));
                }
            }
    
            public string TextLoad
            {
                get => _textLoad;
                set
                {
                    _textLoad = value;
                    OnPropertyChanged(nameof(TextLoad));
                }
            }
    
            public ICommand Click { get; set; }
    
    
            private Random random = new Random();
            private string _textLoad;
    
            private void ChangeData()
            {
                if (_dataPD.DataHelper.ReadUShort(1, Reg_Start) == 1) //*
                {
                    _dataPD.DataHelper.WriteUShort(1, Reg_Start, 0);
                    List<float> list = new List<float>();
                    // Dispatcher.Invoke(() => { Fone.Visibility = Visibility.Visible; });
                    ushort _Address = Address;
                    while (_Address <= Address + 240)
                    {
                        list.Add(_dataPD.DataHelper.ReadFloat(1, _Address));
                        _Address = (ushort) (_Address + 2);
                    }
    
                    List<float[]> listTwoBlock = new List<float[]>();
                    byte tr = 0;
                    float[] block = new float[3];
                    foreach (var value in list)
                    {
                        if (tr < 3)
                        {
                            block[tr] = value;
                        }
                        else
                        {
                            listTwoBlock.Add(block);
                            block = new float[3];
                            block[0] = value;
                            tr = 0;
                        }
    
                        tr++;
                    }
    
                    for (int i = 0; i < listTwoBlock.Count; i++)
                    {
                        /* if (i % 2 == 0)
                            trajectory_1.Add(new Data()
                             {
                                 time = listTwoBlock[i][0],
                                 speed = listTwoBlock[i][1],
                                 acceleration = listTwoBlock[i][2],
                             });
                             
                         else
                             trajectory_2.Add(new Data()
                             {
                                 time = listTwoBlock[i][0],
                                 speed = listTwoBlock[i][1],
                                 acceleration = listTwoBlock[i][2],
                             });
                             
                     }
     */
        /* Dispatcher.Invoke(() => { Fone.Visibility = Visibility.Hidden; });

*/

        //var st = new StackPanel();

        //StringBuilder ff = new StringBuilder();
        //foreach (var f in trajectory_1)
        //{
        //    ff.Append($"{f.time} \n {f.acceleration}\n {f.speed} \n\n");
        //}

        //st.Children.Add(new TextBox()
        //{
        //    TextWrapping = TextWrapping.Wrap,
        //    Text = ff.ToString()
        //});

        //new Window()
        //{
        //    Content = st
        //}.Show();
        // }
        //}
        //  }

        //public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        public static bool Test1()
        {
            // _ftdi.Read
            return false;
        }
    }
}
