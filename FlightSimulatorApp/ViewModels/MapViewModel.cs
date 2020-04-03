using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;


namespace FlightSimulatorApp.ViewModels

{
    public class MapViewModel : INotifyPropertyChanged
    {
        Model.IAircraftModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        //Ctor
        public MapViewModel(Model.IAircraftModel model)
        {
            //Console.WriteLine("In MAP CONSTRUCOR ######################################################################################## ");
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

        public double VM_Latitude { get { return model.Latitude_deg; }  }
        public double VM_Longtitude { get {  return model.Longtitude_deg; } }
        public Location VM_myLoc { get
            {   Console.WriteLine("View" + VM_Latitude);
                return model.myLoc ; } }

    }


}
