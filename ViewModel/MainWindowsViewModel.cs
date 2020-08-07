using DidaktikaApp.Model;
using DidaktikaApp.View.EducationalEquipment.GroupsSolutions.Oscilloscope;
using DidaktikaApplication;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DidaktikaApp.ViewModel
{

    public class MainWindowsViewModel : BaseViewModel
    {
        ModelApp _modelApp = new ModelApp().Deserialize();
        public MainWindowsViewModel()
        {
            //forward properties from the model to view model
            _modelApp.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            GoToSolution = new DelegateCommand<string>(str => { _modelApp.GoToSolution(str); });
            _modelApp.ChangedSolution += OpenSelectedView;
        }

        public DelegateCommand<string> GoToSolution { get; }

        public List<String> ListSolutions => _modelApp.GetAllNameSolutions();

        public void OpenSelectedView(Solution solution)
        {
            Window solutionView = null;
            switch (solution.name)
            {
                case "Осцилограф":
                    solutionView = new OscilloscopeView();
                    break;
            }
            
            if(solutionView != null)
            {
                solutionView.Owner = App.Current.MainWindow;
                solutionView.Show();                
            }  
        }
    }
}
