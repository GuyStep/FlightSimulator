using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace FlightSimulatorApp.Model
{
    public interface IAircraftModel : INotifyPropertyChanged
    {
        void connect(string ip, int port);
        void disconnect();
        void start();


        //Properties that we recieve from the simulator
        double Indicated_heading_deg { set; get; } // indicated-heading-deg
        double Gps_indicated_vertical_speed { set; get; } //gps_indicated-vertical-speed
        double Gps_indicated_ground_speed_kt { set; get; } //gps_indicated-ground-speed-kt
        double Airspeed_indicator_indicated_speed_kt { set; get; } //airspeed-indicator_indicated-speed-kt
        double Gps_indicated_altitude_ft { set; get; } //gps_indicated-altitude-ft
        double Attitude_indicator_internal_roll_deg { set; get; } //attitude-indicator_internal-roll-deg
        double Attitude_indicator_internal_pitch_deg { set; get; } //attitude-indicator_internal-pitch-deg
        double Altimeter_indicated_altitude_ft { set; get; } //altimeter_indicated-altitude-ft

        //Control the aircraft by sending control commands
        void move(double rudder, double aileron, double throttle, double elevator);
    }
}
