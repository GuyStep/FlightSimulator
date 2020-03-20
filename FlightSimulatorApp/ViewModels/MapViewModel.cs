using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModels

{
    class MapViewModel : INotifyPropertyChanged
    {
        Model.IAircraftModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        //Ctor
        public MapViewModel(Model.IAircraftModel model)
        {
            Console.WriteLine("In MAP CONSTRUCOR ######################################################################################## ");
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

            //Console.WriteLine("In NotifyPropertyChanged MAP @@@@@@@@@@@@@@@@@ " );


        }


        //VM aircraft location positions received from model (simulator)

         
        public double VM_Altitude { get { return model.Gps_indicated_vertical_speed; }  }
        public double VM_Longtitude { get { return model.Gps_indicated_ground_speed_kt; } }

    }


}
