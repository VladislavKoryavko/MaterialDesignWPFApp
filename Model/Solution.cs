using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DidaktikaApp.Model
{
    public class Solution
    {
        //private Window view;
        public string name;

        #region Сonstructor
        public Solution()
        {
        }

        public Solution(string name)
        {
            this.name = name;
        }
        #endregion

        public override string ToString()
        {
            return name;
        }
    }
}
