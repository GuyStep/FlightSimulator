﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModels

{
    class JoystickViewModel : INotifyPropertyChanged
    {
        Model.IAircraftModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        private double rudder, aileron, throttle, elevator;

        //Ctor
        public JoystickViewModel(Model.IAircraftModel model)
        {
            Console.WriteLine("In JOYSTICK CONSTRUCOR ######################################################################################## ");
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

        public void moveAircraft(double rudder, double aileron, double throttle, double elevator)
        {
            model.move(rudder, aileron, throttle, elevator);
        }
        /*//VM properties received from view (joystick)
        public double VM_rudder { get { return rudder; } set{ rudder = value; model.move(rudder, aileron, throttle, elevator); } }
        public double VM_aileron { get { return aileron; } set{ aileron = value; model.move(rudder, aileron, throttle, elevator); } }
        public double VM_throttle { get { return throttle; } set{ throttle = value; model.move(rudder, aileron, throttle, elevator); } }
        public double VM_elevator { get { return elevator; } set{ elevator = value; model.move(rudder, aileron, throttle, elevator); } }
*/

    }
}
