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
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : System.Windows.Controls.UserControl
    {

        ViewModels.JoystickViewModel joystickVM;  
        private Point MouseDownLocation;
        bool isPressed = false;
        //public Joystick() { }


        public Joystick(Model.AircraftModel model, ViewModels.JoystickViewModel joystickVM)
        {
            InitializeComponent();
            this.joystickVM = joystickVM;
            DataContext = joystickVM;
        }

        private void CenterKnob_Completed(object sender, EventArgs e) { }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if(!isPressed){
                MouseDownLocation = e.GetPosition(this);
                isPressed = true;

                if (e.Source is Shape shape)
                    shape.CaptureMouse();

            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if(isPressed){
                double x = e.GetPosition(this).X - MouseDownLocation.X;
                double y = e.GetPosition(this).Y - MouseDownLocation.Y;
                if (Math.Sqrt(x * x + y * y) <= InnerCircle.Width / 2)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                    double relativeX = knobPosition.X / InnerCircle.Width * 2;   //Rudder 
                    double relativeY = knobPosition.Y / InnerCircle.Width * (-2);   //Aileron 
                    joystickVM.MoveAircraft(relativeX, relativeY, joystickVM.VM_Throttle, joystickVM.VM_Aileron); //Change the first two properties according to the joystock movement

                }

            }

        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isPressed = false;
            knobPosition.X = 0;
            knobPosition.Y = 0;
            joystickVM.MoveAircraft(0, 0, joystickVM.VM_Throttle, joystickVM.VM_Aileron); //Change the last two properties according to the sliders
            if (e.Source is Shape shape)
                shape.ReleaseMouseCapture();
        }


    }
}
