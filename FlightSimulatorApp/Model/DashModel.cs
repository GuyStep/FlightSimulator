using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Model
{
    public class DashModel 
    {
        


        public event PropertyChangedEventHandler PropertyChanged;
        TcpClient telnetClient; //Need to change to TcpClient
        private NetworkStream stream;
        private BinaryReader reader;
        volatile Boolean stop;
        public DashModel(TcpClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.stop = false;
            this.connect("127.0.0.1", 5402);
            this.start();
        }

        private double indicated_heading_deg;
        private double gps_indicated_vertical_speed;
        private double gps_indicated_ground_speed_kt;
        private double airspeed_indicator_indicated_speed_kt;
        private double gps_indicated_altitude_ft;
        private double attitude_indicator_internal_roll_deg;
        private double attitude_indicator_internal_pitch_deg;
        private double altimeter_indicated_altitude_ft;       


        public double Indicated_heading_deg { get { return indicated_heading_deg; } set { NotifyPropertyChanged("indicated_heading_deg"); indicated_heading_deg = value;} }
        public double Gps_indicated_vertical_speed { get { return gps_indicated_vertical_speed; } set { gps_indicated_vertical_speed = value; NotifyPropertyChanged("gps_indicated_vertical_speed"); } }
        public double Gps_indicated_ground_speed_kt { get { return gps_indicated_ground_speed_kt; } set{ gps_indicated_ground_speed_kt = value; NotifyPropertyChanged("gps_indicated_ground_speed_kt"); } }
        public double Airspeed_indicator_indicated_speed_kt { get { return airspeed_indicator_indicated_speed_kt; } set { airspeed_indicator_indicated_speed_kt = value; NotifyPropertyChanged("airspeed_indicator_indicated_speed_kt"); } }
        public double Gps_indicated_altitude_ft { get { return gps_indicated_altitude_ft; } set { gps_indicated_altitude_ft = value; NotifyPropertyChanged("gps_indicated_altitude_ft"); } }
        public double Attitude_indicator_internal_roll_deg { get { return attitude_indicator_internal_roll_deg; } set { attitude_indicator_internal_roll_deg = value; NotifyPropertyChanged("attitude_indicator_internal_roll_deg"); } }
        public double Attitude_indicator_internal_pitch_deg { get { return attitude_indicator_internal_pitch_deg; } set { attitude_indicator_internal_pitch_deg = value; NotifyPropertyChanged("attitude_indicator_internal_pitch_deg"); } }
        public double Altimeter_indicated_altitude_ft { get { return altimeter_indicated_altitude_ft; } set { altimeter_indicated_altitude_ft = value; NotifyPropertyChanged("altimeter_indicated_altitude_ft"); } }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }


        public void connect(string ip, int port)
        {
            this.telnetClient.Connect(ip, port);
            Console.WriteLine("CONNECTED DASH?");
        }

        public void disconnect()
        {
            this.telnetClient.Close();
        }



        public void write(string command)
        {
            stream = telnetClient.GetStream();
            byte[] send = Encoding.ASCII.GetBytes(command.ToString());
            stream.Write(send, 0, send.Length);

        }

        public string read(TcpClient client)
        {
            reader = new BinaryReader(client.GetStream());
            string input = ""; // input will be stored here
            char s;
            while ((s = reader.ReadChar()) != '\n') input += s;
            return input;
        }

        public void start()
        {

            new Thread(delegate ()
            {
                while (!stop)
                { /*/controls/flight/rudder */
                    this.write("get /instrumentation/heading-indicator/indicated-heading-deg\n"); //Need full path
                    Indicated_heading_deg = Double.Parse(read(telnetClient));
                    Console.WriteLine("Received: " + indicated_heading_deg);
                    Console.WriteLine("Property after UPDATE: " + Indicated_heading_deg);


                    this.write("get /instrumentation/gps/indicated-vertical-speed\n"); //Need full path
                    Gps_indicated_vertical_speed = Double.Parse(read(telnetClient));

                    this.write("get /instrumentation/gps/indicated-ground-speed-kt\n"); //Need full path
                    Gps_indicated_ground_speed_kt = Double.Parse(read(telnetClient));

                    this.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n"); //Need full path
                    Airspeed_indicator_indicated_speed_kt = Double.Parse(read(telnetClient));

                    this.write("get /instrumentation/gps/indicated-altitude-ft\n"); //Need full path
                    Gps_indicated_altitude_ft = Double.Parse(read(telnetClient));

                    this.write("get /instrumentation/attitude-indicator/internal-roll-deg\n"); //Need full path
                    Attitude_indicator_internal_roll_deg = Double.Parse(read(telnetClient));

                    this.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n"); //Need full path
                    Attitude_indicator_internal_pitch_deg = Double.Parse(read(telnetClient));

                    this.write("get /instrumentation/altimeter/indicated-altitude-ft\n"); //Need full path
                    Altimeter_indicated_altitude_ft = Double.Parse(read(telnetClient));



                    Thread.Sleep(250);
                }

            }).Start();
        }
    }
}

