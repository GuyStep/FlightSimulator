using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModels

{
    public class JoystickViewModel : INotifyPropertyChanged
    {
        Model.IAircraftModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        private double rudder = 0, aileron, throttle, elevator = 0;
        public double VM_Throttle { get { return throttle; ; } set { throttle = value; model.Move(rudder, elevator, throttle, aileron); } }
        public double VM_Aileron { get { return aileron; } set { aileron = value; model.Move(rudder, elevator, throttle, aileron); } }


        //Ctor
        public JoystickViewModel(Model.IAircraftModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {

                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }

        public void MoveAircraft(double rudder, double elevator, double throttle, double aileron)
        {
            model.Move(rudder, elevator, this.throttle, this.aileron);
        }
    }
}
