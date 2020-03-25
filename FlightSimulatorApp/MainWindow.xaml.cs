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
        Button mainButton = new Button();
        public MainWindow()
        {
            InitializeComponent();
   /*         
            mainButton.Content = "Connect";
            mainButton.Height = 170;
            mainButton.Width = 500;
            MainGrid.Children.Add(mainButton);*/


            // < Button Name = "MainButton" Content = "Connect" HorizontalAlignment = "Left" Margin = "131,104,0,0" VerticalAlignment = "Top" Width = "528" Grid.ColumnSpan = "2" Height = "176" />

            //           



        }
        private void MainButton_Click(object sender, RoutedEventArgs e)
        {

            View.ConnectWindow win = new View.ConnectWindow();
            win.Show();
            win.setWin(this);
        }

        public void startFlying(string ip, int port)
        {
            MainGrid.Children.Remove(MainButton); 
            model = new Model.AircraftModel(new TcpClient(), ip, port);
            //vm = new ViewModels.AircraftViewModel(model);
            View.Joystick joys = new View.Joystick(model);
            View.DashBoard das = new View.DashBoard(model);
            View.Map mapp = new View.Map(model);

            joySpace.Children.Add(joys);
            dashSpace.Children.Add(das);
            mapSpace.Children.Add(mapp);

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
