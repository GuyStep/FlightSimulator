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
        public Model.AircraftModel model;
        Button mainButton = new Button();
        public MainWindow()
        {
            InitializeComponent();


        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {

            View.ConnectWindow win = new View.ConnectWindow();
            win.Show();
            win.SetWin(this);
        }

        public void StartFlying(string ip, int port)
        {
            MainGrid.Children.Remove(MainButton); 
            model = new Model.AircraftModel(new TcpClient(), ip, port);
            ViewModels.DashBoardViewModel VMdashboard = new ViewModels.DashBoardViewModel(model);
            View.DashBoard das = new View.DashBoard(model, VMdashboard);
            dashSpace.Children.Add(das);
            DataContext = VMdashboard;
            if (!model.Stop){
                ViewModels.JoystickViewModel VMjoystick = new ViewModels.JoystickViewModel(model);
                ViewModels.MapViewModel VMmap = new ViewModels.MapViewModel(model);

                View.Sliders slide = new View.Sliders(model, VMjoystick);
                View.Map mapp = new View.Map(model, VMmap);

                joySpace.Children.Add(slide);
                mapSpace.Children.Add(mapp);
            }


        }

        private void Joystick_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

