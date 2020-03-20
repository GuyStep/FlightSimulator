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


namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        ViewModels.MapViewModel mapVM;

        public Map(Model.AircraftModel model)
        {
            InitializeComponent();
            this.mapVM = new ViewModels.MapViewModel(model);
            DataContext = mapVM;
            Console.WriteLine("View" + mapVM.VM_Altitude);
        }

    }
}
