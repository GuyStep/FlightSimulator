using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Maps.MapControl.WPF;


namespace FlightSimulatorApp.Model
{
    public class AircraftModel : IAircraftModel
    {

        private static Mutex mut = new Mutex();


        public event PropertyChangedEventHandler PropertyChanged;
        TcpClient telnetClient; //Need to change to TcpClient
        private NetworkStream stream;
        private BinaryReader reader;
        public Boolean stop;
        public AircraftModel(TcpClient telnetClient, string ip, int port)
        {
            this.telnetClient = telnetClient;
            telnetClient.SendTimeout = 10000;
            this.stop = false;
            this.connect(ip, port);
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
        private double latitude_deg;
        private double longtitude_deg;
        private double throttle;
        private double aileron;
        private string error = "None";
        private Location loc;

        public double Indicated_heading_deg { get { return indicated_heading_deg; } set { NotifyPropertyChanged("indicated_heading_deg"); indicated_heading_deg = value; } }
        public double Gps_indicated_vertical_speed { get { return gps_indicated_vertical_speed; } set { gps_indicated_vertical_speed = value; NotifyPropertyChanged("gps_indicated_vertical_speed"); } }
        public double Gps_indicated_ground_speed_kt { get { return gps_indicated_ground_speed_kt; } set { gps_indicated_ground_speed_kt = value; NotifyPropertyChanged("gps_indicated_ground_speed_kt"); } }
        public double Airspeed_indicator_indicated_speed_kt { get { return airspeed_indicator_indicated_speed_kt; } set { airspeed_indicator_indicated_speed_kt = value; NotifyPropertyChanged("airspeed_indicator_indicated_speed_kt"); } }
        public double Gps_indicated_altitude_ft { get { return gps_indicated_altitude_ft; } set { gps_indicated_altitude_ft = value; NotifyPropertyChanged("gps_indicated_altitude_ft"); } }
        public double Attitude_indicator_internal_roll_deg { get { return attitude_indicator_internal_roll_deg; } set { attitude_indicator_internal_roll_deg = value; NotifyPropertyChanged("attitude_indicator_internal_roll_deg"); } }
        public double Attitude_indicator_internal_pitch_deg { get { return attitude_indicator_internal_pitch_deg; } set { attitude_indicator_internal_pitch_deg = value; NotifyPropertyChanged("attitude_indicator_internal_pitch_deg"); } }
        public double Altimeter_indicated_altitude_ft { get { return altimeter_indicated_altitude_ft; } set { altimeter_indicated_altitude_ft = value; NotifyPropertyChanged("altimeter_indicated_altitude_ft"); } }
        public double Latitude_deg { get { return latitude_deg; } set { latitude_deg = value; NotifyPropertyChanged("latitude_deg"); } }
        public double Longtitude_deg { get { return longtitude_deg; } set { longtitude_deg = value; NotifyPropertyChanged("longtitude_deg"); } }
        public Location myLoc { get { return loc; } set { loc = value; NotifyPropertyChanged("myLoc"); } }
        public double Throttle { get { return throttle; } set { throttle = value; NotifyPropertyChanged("throttle"); } }
        public double Aileron { get { return aileron; } set { aileron = value; NotifyPropertyChanged("aileron"); } }
        public string Error { get { return error; } set { error = value; NotifyPropertyChanged("error"); } }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }


        public void connect(string ip, int port)
        {
            try
            {
                this.telnetClient.Connect(ip, port);
            }
            catch
            {
                Error = "Could not connect to server";
                this.stop = true;

            }
            Console.WriteLine("CONNECTED)?");
        }

        public void disconnect()
        {
            this.telnetClient.Close();
        }

        public void move(double rudder, double elevator, double throttle, double aileron)
        {
            mut.WaitOne();
            this.write("set /controls/flight/rudder " + rudder + "\n");
            Double.Parse(read(telnetClient));
            this.write("set /controls/flight/elevator " + elevator + "\n");
            Double.Parse(read(telnetClient));
            this.write("set /controls/flight/aileron " + aileron + "\n");
            Double.Parse(read(telnetClient));
            this.write("set /controls/engines/current-engine/throttle " + throttle + "\n");
            Double.Parse(read(telnetClient));

            //Console.WriteLine("Sent: "+rudder);
            //Console.WriteLine("Received after set: "+);
            mut.ReleaseMutex();


        }

        public void write(string command)
        {
            try { stream = telnetClient.GetStream();
                byte[] send = Encoding.ASCII.GetBytes(command.ToString());
                stream.Write(send, 0, send.Length);
            }
            catch
            {
                Error = "Couldn't write to server.";
            }



        }

        public string read(TcpClient client)
        {

                string input = ""; // input will be stored here
            try { 
                reader = new BinaryReader(client.GetStream());
                char s;
                while ((s = reader.ReadChar()) != '\n') input += s;
            }
            catch {
                Error = "Couldn't recieve from server.";
                return "0";
            }

            try { Double.Parse(input);
                return input;
            }
            catch
            {
                Error = "Wrong value returned from server";
                return "0";
            }

        }

        public void start()
        {
            string input;
            double offset = 0.001;
            new Thread(delegate ()
            {
                while (!stop)
                { /*/instrumentation/heading-indicator/indicated-heading-deg*/
                    Random rnd = new Random();
                    mut.WaitOne();

                    this.write("get /controls/flight/rudder\n"); //Need full path
                    Indicated_heading_deg = Double.Parse(read(telnetClient));
                    //Console.WriteLine("Received: " + indicated_heading_deg);
                    //Console.WriteLine("Property after UPDATE: " + Indicated_heading_deg);

                    /*                   string[] arr = s.Split(' ');
                                       string result = arr[2];
                                       result = result.Replace("\n", string.Empty);*/
                    /*                    double num = Double.Parse(s);
                                        Indicated_heading_deg = num;*/


                    /*Indicated_heading_deg = rnd.NextDouble();*/

                    /*                    Console.WriteLine("Received: " + indicated_heading_deg);
                                        Console.WriteLine("Property after UPDATE: " + Indicated_heading_deg);*/


                    this.write("get /instrumentation/gps/indicated-vertical-speed\n");
                    input = read(telnetClient);
                    Gps_indicated_vertical_speed = Double.Parse(input); ;

                    this.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    input = read(telnetClient);
                    Gps_indicated_ground_speed_kt = Double.Parse(input);

                    this.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    input = read(telnetClient);
                    Airspeed_indicator_indicated_speed_kt = Double.Parse(input);

                    this.write("get /instrumentation/gps/indicated-altitude-ft\n");
                    input = read(telnetClient);
                    Gps_indicated_altitude_ft = Double.Parse(input);

                    this.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    input = read(telnetClient);
                    Attitude_indicator_internal_roll_deg = Double.Parse(input);

                    this.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    input = read(telnetClient);
                    Attitude_indicator_internal_pitch_deg = Double.Parse(input);

                    this.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    input = read(telnetClient);
                    Altimeter_indicated_altitude_ft = Double.Parse(input);

                    double tempLat, tempLon;
                    this.write("get /position/latitude-deg\n");
                    input = read(telnetClient);
                    tempLat = Double.Parse(input) + offset; //Latitude_deg

                    this.write("get /position/longitude-deg\n");
                    input = read(telnetClient);
                    tempLon = Double.Parse(input) + offset; //Longtitude_deg
                    offset = offset + 0.0001;
                    if (tempLon > 180)                   
                    {
                        Error = "Longtitude degree above 180";
                        Latitude_deg = 180;
                    }
                    if ( tempLon < -180)
                    {
                        Error = "Longtitude degree below -180";
                        Latitude_deg = -180;
                    }
                    if (tempLat > 90)
                    {
                        Error = "Latitude degree above 90";
                        Longtitude_deg = 90;
                    }
                    if ( tempLat < -90)
                    {
                        Error = "Latitude degree below -90";
                        Longtitude_deg = -90;
                    }


                    myLoc = new Location(Latitude_deg, Longtitude_deg);


                    mut.ReleaseMutex();
                    Thread.Sleep(250);


                }

            }).Start();
        }
    }
}