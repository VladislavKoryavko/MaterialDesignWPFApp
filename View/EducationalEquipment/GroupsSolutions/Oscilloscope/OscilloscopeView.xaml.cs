using DidaktikaApp.ViewModel.EducationalEquipment.Oscilloscope;
using OxyPlot;
using OxyPlot.Annotations;
using System;
using System.Windows;
using System.Windows.Input;

namespace DidaktikaApp.View.EducationalEquipment.GroupsSolutions.Oscilloscope
{
    /// <summary>
    /// Логика взаимодействия для OscilloscopeView.xaml
    /// </summary>
    public partial class OscilloscopeView : Window
    {
        public OscilloscopeView()
        {
            InitializeComponent();
            DataContext = new OscilloscopeViewModel();

            OxyPlotOscilloscope.MouseDown += Plott_MouseDown;
            OxyPlotOscilloscope.MouseLeftButtonDown += Plott_MouseLeftButtonDown;
            OxyPlotOscilloscope.PreviewMouseDown += Plott_PreviewMouseDown;
            //OxyPlotOscilloscope.MoveFocus
           // OxyPlotOscilloscope.Axes.Add(new Oxy.LinearAxis);
            
        }

        private void Plott_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Plott_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void Plott_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        

        //public void Plott_MouseDown(object sender, OxyMouseDownEventArgs e)
        //{
        //    //OxyPlotOscilloscope.Annotations.Add((new RectangleAnnotation() { MinimumX = e.Position.X, MinimumY = e.Position.Y }));
        //     //   Annotations.Add(new Annotation.() { MinimumX = e.Position.X, MinimumY = e.Position.Y });
        //}
    }
}
