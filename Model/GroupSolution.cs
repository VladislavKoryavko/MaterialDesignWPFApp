using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DidaktikaApp.Model
{
    public class GroupSolution
    {
        public string name;

        public ObservableCollection<Solution> allSolutions;

        #region Сonstructor
        public GroupSolution()
        {
        }
        public GroupSolution(string name)
        {
            this.name = name;
        }
        #endregion

    }
}
