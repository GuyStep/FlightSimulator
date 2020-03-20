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

        public Joystick(Model.AircraftModel model)
        {
            InitializeComponent();
            joystickVM = new ViewModels.JoystickViewModel(model);
            DataContext = joystickVM;
            //Console.WriteLine(joystickVM.VM_Indicated_heading_deg);
        }

        private void centerKnob_Completed(object sender, EventArgs e) { }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
/*            if (e.ChangedButton == MouseButton.Left)
*/            
            if(!isPressed){
                MouseDownLocation = e.GetPosition(this);
                isPressed = true;

                if (e.Source is Shape shape)
                    shape.CaptureMouse();

            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
/*            if (e.LeftButton == MouseButtonState.Pressed)
*/            if(isPressed){
                double x = e.GetPosition(this).X - MouseDownLocation.X;
                double y = e.GetPosition(this).Y - MouseDownLocation.Y;
                if (Math.Sqrt(x * x + y * y) <= InnerCircle.Width / 2)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                    joystickVM.moveAircraft(1, 1, 1, 1); //Change the first two properties according to the joystock movement

                }

            }

        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isPressed = false;
            knobPosition.X = 0;
            knobPosition.Y = 0;
            joystickVM.moveAircraft(0, 0, 1, 1); //Change the last two properties according to the bars
            if (e.Source is Shape shape)
                shape.ReleaseMouseCapture();
        }


    }
}
