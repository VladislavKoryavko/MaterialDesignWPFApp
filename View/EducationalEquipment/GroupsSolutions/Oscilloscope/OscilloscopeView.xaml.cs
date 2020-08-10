using DidaktikaApp.ViewModel.EducationalEquipment.Oscilloscope;
using System.Windows;

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
        }
    }
}
