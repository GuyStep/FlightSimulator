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
using System.Windows.Shapes;

namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        MainWindow win;
        public ConnectWindow()
        {
            InitializeComponent();
        }

        public void setWin(MainWindow win)
        {
            this.win = win;
        }
        private void conButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                win.startFlying(ipText.Text, Int32.Parse(portText.Text));
            }
            catch
            {
                win.startFlying("127.0.0.1", 5402);
            }
            this.Close();
        }

    }
}
