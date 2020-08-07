using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DidaktikaApp.Model
{
    [Serializable]
    public class ModelApp : BindableBase
    {
        public string name;
        private string pathModelInformation = Environment.CurrentDirectory + "\\Model\\ModelApp.xml";

        public ObservableCollection<GroupSolution> allGroupsSolutions = new ObservableCollection<GroupSolution>();
        
        private Solution selectSolution;
        public Solution SelectSolution 
        { 
            get => selectSolution;
            set
            {
                selectSolution = value;
                RaisePropertyChanged("SelectSolution");
            }
        }

        public delegate void ChangSolution(Solution selectedSolution);
        public event ChangSolution ChangedSolution;

        #region Сonstructor
        public ModelApp()
        {
            
        }

        public ModelApp(string name)
        {
            this.name = name;
            allGroupsSolutions = new ObservableCollection<GroupSolution> {
                new GroupSolution { name = "Group1", allSolutions = new ObservableCollection<Solution> {new Solution("ExtLab")} },
                new GroupSolution { name = "Group2", allSolutions = new ObservableCollection<Solution> { new Solution("Осцилограф")} },
                new GroupSolution { name = "Group3", allSolutions = new ObservableCollection<Solution> { new Solution("TechMeh"), new Solution("FizMeh") } } };
        }
        #endregion

        #region Serialization 
        //Load in file university information
        public void Serialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ModelApp));
            TextWriter tw = new StreamWriter(@pathModelInformation);
            xs.Serialize(tw, this);
            tw.Close();
        }

        //Load from file university information
        public ModelApp Deserialize()
        {
            //Load from file university information
            XmlSerializer xs = new XmlSerializer(typeof(ModelApp));
            using (var sr = new StreamReader(@pathModelInformation))
            {
                return (ModelApp)xs.Deserialize(sr);
            }
        }
        #endregion

        public List<string> GetAllNameSolutions()
        {
            List<string> allNames = new List<string>();
            foreach (GroupSolution group in allGroupsSolutions)
            {
                foreach(Solution solution in group.allSolutions)
                {
                    allNames.Add(solution.name);
                } 
            }
            return allNames;
        }
        public void GoToSolution(string name)
        {
            ObservableCollection<Solution> suitableSolutions = new ObservableCollection<Solution>();

            foreach (GroupSolution group in allGroupsSolutions)
            {
                foreach (Solution solution in group.allSolutions)
                {
                    if (solution.name == name)
                    {
                        suitableSolutions.Add(solution);
                    }
                }
            }
            if (suitableSolutions.Count() > 0)
            {
                SelectSolution = suitableSolutions.First();
                ChangedSolution?.Invoke(SelectSolution);
            }

        }
    }


}
