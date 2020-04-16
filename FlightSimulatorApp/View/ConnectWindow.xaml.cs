using System;
using System.Collections.Generic;
using System.Configuration;
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
        string ipaddress;
        string portnum;
        string error = "";
        public string Error { get { return error; } set { error = value; }  }
        public string IPaddress { get { return ipaddress; } set { ipaddress = value; }  }
        public string PORTnum { get { return portnum; } set { portnum = value; }  }

        public ConnectWindow()
        {
            InitializeComponent();
            DataContext = this;
            IPaddress = ConfigurationManager.AppSettings["ip"];
            PORTnum = ConfigurationManager.AppSettings["port"];

        }

        public void SetWin(MainWindow win)
        {
            this.win = win;
        }
        private void ConButton_Click(object sender, RoutedEventArgs e)
        {
            Label errorLabel = new Label();
            errorLabel.Margin = new System.Windows.Thickness(41, 34, 0, 0);
            errorLabel.Height = 26;
            griddd.Children.Add(errorLabel);

           // errorLabel.



            try
            {
                win.StartFlying(ipText.Text, Int32.Parse(portText.Text));
                this.Close();

             }
            catch
             {
                errorLabel.Content = "Bad ip or port, try again";

            }

        }

    }
}
