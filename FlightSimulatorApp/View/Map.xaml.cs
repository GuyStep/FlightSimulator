using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using Microsoft.Maps.MapControl.WPF;


namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        ViewModels.MapViewModel mapVM;

        public Map(Model.AircraftModel model, ViewModels.MapViewModel mapVM)
        {
            InitializeComponent();
            this.mapVM = mapVM;
            DataContext = mapVM;
            //Location loc = new Location(mapVM.VM_Latitude, mapVM.VM_Longtitude);

           



        }

    }
}
