﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModels

{
    public class DashBoardViewModel : INotifyPropertyChanged
    {
        Model.IAircraftModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        //Ctor
        public DashBoardViewModel(Model.IAircraftModel model)
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


        //VM properties received from model (simulator)
        public double VM_Indicated_heading_deg { get { return model.Indicated_heading_deg; 
            }
        } 
        public double VM_Gps_indicated_vertical_speed { get { return model.Gps_indicated_vertical_speed; }  }
        public double VM_Gps_indicated_ground_speed_kt { get { return model.Gps_indicated_ground_speed_kt; } }
        public double VM_Airspeed_indicator_indicated_speed_kt { get { return model.Airspeed_indicator_indicated_speed_kt; }  }
        public double VM_Gps_indicated_altitude_ft { get { return model.Gps_indicated_altitude_ft; }  }
        public double VM_Attitude_indicator_internal_roll_deg { get { return model.Attitude_indicator_internal_roll_deg; }  }
        public double VM_Attitude_indicator_internal_pitch_deg { get { return model.Attitude_indicator_internal_pitch_deg; }  }
        public double VM_Altimeter_indicated_altitude_ft { get { return model.Altimeter_indicated_altitude_ft; } }
        public string VM_Error { get { return model.Error; } }
    }


}
