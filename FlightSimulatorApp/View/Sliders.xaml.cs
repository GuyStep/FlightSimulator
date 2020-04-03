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

namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for Sliders.xaml
    /// </summary>
    public partial class Sliders : UserControl
    {

        ViewModels.JoystickViewModel joystickVM;
        private Point MouseDownLocation;
        bool isPressed = false;
        //public Joystick() { }
        private double aileron, throttle;

        public Sliders(Model.AircraftModel model, ViewModels.JoystickViewModel joystickVM)
        {
            InitializeComponent();
            this.joystickVM = joystickVM; //Needs to be passed ffrom mainwindow
            DataContext = joystickVM;
            Joystick joys = new Joystick(model, joystickVM);
            joystickSpace.Children.Add(joys);
            //Console.WriteLine(joystickVM.VM_Indicated_heading_deg);
        }

        private void throttle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
