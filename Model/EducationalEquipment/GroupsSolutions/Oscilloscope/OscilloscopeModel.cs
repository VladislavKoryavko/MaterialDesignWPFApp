using Didaktika.PD.FTDI;
using Didaktika.PD.FTDI.Models;
using DidaktikaApp.Helpers.Utils.Serelization;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Connect;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelGraphic;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.ModelPoints;
using DidaktikaApp.Model.EducationalEquipment.GroupsSolutions.Oscilloscope.Settings;
using FTD2XX_NET;
using LiveCharts;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DidaktikaApp.Model.EducationalEquipment.Oscilloscope
{
    public class OscilloscopeModel : BindableBase
    {

        private bool OnLine = false;

        #region Setting control panel
        #region Voltage dividers
        public static List<VoltageDivider> VoltageScaleDividers { get; set; } = new List<VoltageDivider> {
            new VoltageDivider(0, "200 mV", 0.89, "mV", -200, 200, 20),
            new VoltageDivider(1, "450 mV", 1.7647, "mV", -450, 450, 50),
            new VoltageDivider(2, "1 V", 4.466, "mV", -1000 , 1000, 100),
            new VoltageDivider(3, "2 V", 8.9, "mV", -2000, 2000, 200),
            new VoltageDivider(4, "4.5 V", 0.017647, "V", -4.5, 4.5, 0.5),
            new VoltageDivider(5, "10 V", 0.0443, "V", -10, 10, 1),
            new VoltageDivider(6, "20 V", 0.089, "V", -20, 20, 2),
            new VoltageDivider(7, "45 V", 0.1764, "V", -45, 45, 5),
            new VoltageDivider(8, "100 V", 0.3921, "V", -100, 100, 10),
            new VoltageDivider(9, "200 V", 0.7843, "V", -200, 200, 20) };


        private VoltageDivider selectVoltageScaleDividerA = VoltageScaleDividers[0]; 
        private VoltageDivider selectVoltageScaleDividerB = VoltageScaleDividers[0]; 

        public VoltageDivider SelectVoltageScaleDividerA
        {
            get => selectVoltageScaleDividerA;
            set
            {
                selectVoltageScaleDividerA = value;
                //Set settings byte voltage divider for forward to oscilloscope
                byte setByte = (byte)((SettinsForForward[(int)SettingsBytes.DivVoltage] & 0b11110000) | value.ForSend);
                SettinsForForward[(int)SettingsBytes.DivVoltage] = setByte;
                RaisePropertyChanged(nameof(SelectVoltageScaleDividerA));
            }
        }
        public VoltageDivider SelectVoltageScaleDividerB
        {
            get => selectVoltageScaleDividerB;
            set
            {
                selectVoltageScaleDividerB = value;
                byte setByte = (byte)((SettinsForForward[(int)SettingsBytes.DivVoltage] & 0b00001111) | value.ForSend << 4);
                SettinsForForward[(int)SettingsBytes.DivVoltage] = setByte;
                RaisePropertyChanged(nameof(SelectVoltageScaleDividerB));
            }
        }
        #endregion

        #region Trigger
        public static List<TriggerOscillosxope> TriggerStates { get; set; } = new List<TriggerOscillosxope> (){ 
            new TriggerOscillosxope(0b00011100, "Нет"), 
            new TriggerOscillosxope(0b00011100, "Канал А (по нарастанию)"), 
            new TriggerOscillosxope(0b00011100, "Канал А (по спаду)"), 
            new TriggerOscillosxope(0b00011100, "Канал Б (по нарастанию)"), 
            new TriggerOscillosxope(0b00011100, "Канал Б (по спаду)"), };

        private TriggerOscillosxope selectTriger = TriggerStates[0];
        public TriggerOscillosxope SelectTriger 
        { 
            get => selectTriger;
            set
            {
                if(value == null)
                {
                    value = TriggerStates[0];
                }
                selectTriger = value;
                //Set settings byte "settings1" for forward to oscilloscope
                byte setByte = (byte)((SettinsForForward[(int)SettingsBytes.Settings1] & 0b11100011) | value.ForSend);
                SettinsForForward[(int)SettingsBytes.Settings1] = setByte;
                RaisePropertyChanged(nameof(SelectTriger));
                RaisePropertyChanged(nameof(TriggerStates));
            }
        }

        private ushort triggerPositionAxisX = 0;
        private byte triggerPositionAxisY = 0;
        public ushort TriggerPositionAxisX { get => triggerPositionAxisX; set => triggerPositionAxisX = value; }
        public byte TriggerPositionAxisY { get => triggerPositionAxisY; set => triggerPositionAxisY = value; }

        #endregion

        public List<string> TypeChanelCurent = new List<string> { "AC", "DC" };
        public List<string> Filtrations = new List<string>();
        #endregion

        #region OxyPlot
        private string _titleForAxisX = "S";
        public string TitleForAxisX { get => _titleForAxisX; set => _titleForAxisX = value; }

        private ModelGraphic _graphicA;
        public ModelGraphic GraphicA
        {
            get { return _graphicA; }
            set
            {
                _graphicA = value;
                RaisePropertyChanged(nameof(GraphicA));
            }
        }

        private ModelGraphic _graphicB;
        public ModelGraphic GraphicB
        {
            get { return _graphicB; }
            set
            {
                _graphicB = value;
                RaisePropertyChanged(nameof(GraphicB));
            }
        }
        #endregion

        #region Connection
        public FTDISearcher ftdiSearcher;
        public IEnumerable<DeviceInfo> ftdiCollection;
        public FTDIConnection ftdiConnection;

        private Task ReadData;
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;

        private SettingsConnections _settingsConnections;
        public SettingsConnections SettingsConnections
        {
            get => _settingsConnections;
            set => _settingsConnections = value;
        }

        #endregion

        #region Data Out/In
        private byte[] settingsForForward;
        public byte[] SettinsForForward { get => settingsForForward; set => settingsForForward = value; }
       

        private byte[] dataFromOscillograf;

        #endregion



        #region Constructors
        public OscilloscopeModel()
        {
            Filtrations.Clear();
            for (int i = 0; i < 100; i++)
            {
                // Filtrations.Add((string)i);
            };

            SettingsConnections = UtilSerialized.Deserialized<SettingsConnections>(Environment.CurrentDirectory + "\\Model\\EducationalEquipment\\GroupsSolutions\\Oscilloscope\\Settings\\SettingConnections.xml");
            if (SettingsConnections == null || SettingsConnections.CountBits == 0 || SettingsConnections.LengthPackage == 0 || SettingsConnections.BaudRate == 0 || SettingsConnections.BitsToSend == null)
            {
                SetDefoultSettings();
            }
            else
            {
                settingsForForward = SettingsConnections.BitsToSend.Select(s => byte.Parse(s, NumberStyles.AllowHexSpecifier)).ToArray();
            }

            dataFromOscillograf = new byte[SettingsConnections.LengthPackage];

            //SelectVoltageScaleDividerA = new VoltageDivider(0, "200 mV");
        }
        #endregion


        #region Management functions
        public void SetDefoultSettings()
        {
            SettingsConnections.CountBits = 20;
            SettingsConnections.LengthPackage = 1007;
            SettingsConnections.BaudRate = 921600;
            SettingsConnections.BitsToSend = new ObservableCollection<string>() { "AA", "FF", "FF", "00", "00", "cf", "83", "7f", "20", "f4", "01", "7f", "7f", "7f", "25", "c3", "66", "6f", "00", "01" };
            settingsForForward = SettingsConnections.BitsToSend.Select(s => byte.Parse(s, NumberStyles.AllowHexSpecifier)).ToArray();

            SettingsConnections.DataBits = 8;
            SettingsConnections.StopBits = 2;
            SettingsConnections.Parity = 4;
        }

        public bool SetConnedtion()
        {
            if (ReadData != null)
            {
                if (!ReadData.IsCompleted)
                {
                    cancelTokenSource = new CancellationTokenSource();
                    token = cancelTokenSource.Token;
                    cancelTokenSource.Cancel();
                    while (!ReadData.IsCompleted)
                    { }
                }
            }
            ftdiSearcher = new FTDISearcher();
            uint countFTDI = ftdiSearcher.DeviceCount;
            ftdiCollection = ftdiSearcher.GetAllDevices();
            if (countFTDI > 0)
            {
                //if (ftdiCollection.First().Status != DeviceStatus.Open)
                try
                {
                    ftdiConnection = ftdiSearcher.OpenDevice(ftdiCollection.First(),
                        SettingsConnections.BaudRate, DataBits.Bits8, StopBits.Two, Parity.Space);
                }
                catch
                {

                }
                return ftdiCollection.First().Status == DeviceStatus.Open ? true : false;
            }
            else
            {
                return false;
            }
        }

        public void StartConnection()
        {
            RaisePropertyChanged("SelectVoltageScaleDividerA");
            // if connection open
            if (SetConnedtion())
            {
                OnLine = true;
                cancelTokenSource = new CancellationTokenSource();
                token = cancelTokenSource.Token;
                //cancelTokenSource.Cancel();

                ReadData = new Task(() =>
                {
                    while (OnLine && ftdiCollection.First().Status == DeviceStatus.Open)
                    {
                        try
                        {
                            ftdiConnection.WriteArrayBytes(SettinsForForward);
                            dataFromOscillograf = ftdiConnection.ReadArrayBytes(SettingsConnections.LengthPackage);
                        }
                        catch
                        {
                        }

                        int count = 0;
                        int countA = 0;
                        int countB = 0;

                        ObservableCollection<ModelPoint> ListValuesOnGraphicChanelA = new ObservableCollection<ModelPoint>();
                        ObservableCollection<ModelPoint> ListValuesOnGraphicChanelB = new ObservableCollection<ModelPoint>();

                        foreach (byte point in dataFromOscillograf)
                        {
                            if (count >= 6 && count <= 505)
                            {
                                ListValuesOnGraphicChanelB.Add(new ModelPoint(countB++, (point - 127) * SelectVoltageScaleDividerB.CoefficientDivider));
                            }
                            if (count > 506 && count <= 1005)
                            {
                                ListValuesOnGraphicChanelA.Add(new ModelPoint(countA++, (point - 127) * SelectVoltageScaleDividerA.CoefficientDivider));
                            }
                            count++;
                        }
                        GraphicA = new ModelGraphic() { ListValuesOnGraphic = ListValuesOnGraphicChanelA };
                        GraphicB = new ModelGraphic() { ListValuesOnGraphic = ListValuesOnGraphicChanelB };

                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                });
                ReadData.Start();
            }
            else
            {
                MessageBox.Show("Подключите устройство и повторите попытку");
            }

        }

        public void StopConnection()
        {
            OnLine = false;
            cancelTokenSource.Cancel();
        }
        #endregion

    }

    public enum SettingsBytes : int
    {
        Settings1 = 15,    //Делитель напряжения
        DivVoltage = 16,    //Делитель напряжения

    }
}
