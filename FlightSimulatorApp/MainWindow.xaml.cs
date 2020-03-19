using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModels.AircraftViewModel vm;
        Model.AircraftModel model;

        public MainWindow()
        {
            model = new Model.AircraftModel(new TcpClient());
            //vm = new ViewModels.AircraftViewModel(new Model.AircraftModel(new TcpClient()));
            new View.Joystick(/*model*/);
            new View.DashBoard(/*model*/);
            //InitializeComponent();

        }

        private void Joystick_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}


/*    {
        #region Singleton
        private static TcpClient m_Instance = null;
        public static TcpClient Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new TcpClient();
                }
                return m_Instance;
            }
        }
        #endregion*/
