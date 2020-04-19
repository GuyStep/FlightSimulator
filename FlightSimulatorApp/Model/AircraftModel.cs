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
        TcpClient telnetClient;
        private NetworkStream stream;
        private BinaryReader reader;
        public Boolean Stop;
        public AircraftModel(TcpClient telnetClient, string ip, int port)
        {
            this.telnetClient = telnetClient;
            telnetClient.ReceiveTimeout = 10000;
            this.Stop = false;
            this.Connect(ip, port);
            this.Start();
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
        public Location MyLoc { get { return loc; } set { loc = value; NotifyPropertyChanged("myLoc"); } }
        public double Throttle { get { return throttle; } set { throttle = value; NotifyPropertyChanged("throttle"); } }
        public double Aileron { get { return aileron; } set { aileron = value; NotifyPropertyChanged("aileron"); } }
        public string Error { get { return error; } set { error = value; NotifyPropertyChanged("error"); } }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }


        public void Connect(string ip, int port)
        {
            try
            {
                this.telnetClient.Connect(ip, port);
            }
            catch
            {
                Error = "Could not connect to server";
                this.Stop = true;

            }
        }

        public void Disconnect()
        {
            this.telnetClient.Close();
        }

        //Sends joystick and slider values to the server
        public void Move(double rudder, double elevator, double throttle, double aileron)
        {

            new Thread(delegate () //Uses own thread in order to avoid ui freezes
            {
                if (!Stop)
                {

                    mut.WaitOne();
                    this.Write("set /controls/flight/rudder " + rudder + "\n");
                    Double.Parse(Read(telnetClient));
                    this.Write("set /controls/flight/elevator " + elevator + "\n");
                    Double.Parse(Read(telnetClient));
                    this.Write("set /controls/flight/aileron " + aileron + "\n");
                    Double.Parse(Read(telnetClient));
                    this.Write("set /controls/engines/current-engine/throttle " + throttle + "\n");
                    Double.Parse(Read(telnetClient));

                    mut.ReleaseMutex();
                }
            }).Start();

        }


        //writes to simulator 
        public void Write(string command)
        {

            try
            {
                stream = telnetClient.GetStream();
                stream.Flush();
                byte[] send = Encoding.ASCII.GetBytes(command.ToString());
                stream.Write(send, 0, send.Length);
            }
            catch
            {
                if (!Stop)
                    Error = "Couldn't write to server.";
                else
                    Error = "Disconnected from server (error/timeout)";
            }



        }

        //Reads from simulator 
        public string Read(TcpClient client)
        {

            string input = ""; // input will be stored here
            try
            {
                reader = new BinaryReader(client.GetStream());
                char s;
                while ((s = reader.ReadChar()) != '\n') input += s;
            }

            catch (IOException e)
            {
                if (e.Message.Contains("time"))
                {
                    Error = "Disconnected from server (error/timeout)";
                    Stop = true;
                    Disconnect();
                }

            }
            catch
            {
                if (!Stop)
                    Error = "Couldn't recieve from server.";
                return "0";
            }

            try
            {
                Double.Parse(input);
                return input;
            }
            catch
            {
                if (!Stop)
                    Error = "Wrong value returned from server";
                return "0";
            }

        }

        public void Start()
        {
            string input;
            double offset = 0.001;
            new Thread(delegate () //Runs a thread that gets all dashboard meters from simulators (in a loop)
            {
                while (!Stop)
                { /*/controls/flight/rudder*/
                    Random rnd = new Random();
                    //Thread.Sleep(8000);
                    mut.WaitOne();

                    this.Write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                    Indicated_heading_deg = Double.Parse(Read(telnetClient));


                    this.Write("get /instrumentation/gps/indicated-vertical-speed\n");
                    input = Read(telnetClient);
                    Gps_indicated_vertical_speed = Double.Parse(input);

                    this.Write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    input = Read(telnetClient);
                    Gps_indicated_ground_speed_kt = Double.Parse(input);

                    this.Write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    input = Read(telnetClient);
                    Airspeed_indicator_indicated_speed_kt = Double.Parse(input);

                    this.Write("get /instrumentation/gps/indicated-altitude-ft\n");
                    input = Read(telnetClient);
                    Gps_indicated_altitude_ft = Double.Parse(input);

                    this.Write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    input = Read(telnetClient);
                    Attitude_indicator_internal_roll_deg = Double.Parse(input);

                    this.Write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    input = Read(telnetClient);
                    Attitude_indicator_internal_pitch_deg = Double.Parse(input);

                    this.Write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    input = Read(telnetClient);
                    Altimeter_indicated_altitude_ft = Double.Parse(input);

                    double tempLat, tempLon;
                    this.Write("get /position/latitude-deg\n");
                    input = Read(telnetClient);
                    tempLat = Double.Parse(input) + offset; //Latitude_deg

                    this.Write("get /position/longitude-deg\n");
                    input = Read(telnetClient);
                    tempLon = Double.Parse(input) + offset; //Longtitude_deg
                    offset = offset + 0.0001;
                    Longtitude_deg = tempLon;
                    Latitude_deg = tempLat;
                    //Boundries of location (map related constraints)
                    if (tempLon > 180)
                    {
                        Error = "Longtitude degree above 180";
                        Longtitude_deg = 180;
                    }
                    if (tempLon < -180)
                    {
                        Error = "Longtitude degree below -180";
                        Longtitude_deg = -180;
                    }
                    if (tempLat > 90)
                    {
                        Error = "Latitude degree above 90";
                        Latitude_deg = 90;
                    }
                    if (tempLat < -90)
                    {
                        Error = "Latitude degree below -90";
                        Latitude_deg = -90;
                    }


                    MyLoc = new Location(Latitude_deg, Longtitude_deg);
                    Console.Out.Write(loc);



                    mut.ReleaseMutex();
                    Thread.Sleep(250);


                }

            }).Start();
        }
    }
}