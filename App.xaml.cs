using DidaktikaApp.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DidaktikaApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            //For start initializations model.xml
            ModelApp modelInformationsStart = new ModelApp("StartInf");
            modelInformationsStart.Serialize();

            //ModelApp modelInformations = new ModelApp().Deserialize();

        }

        
    }
}
