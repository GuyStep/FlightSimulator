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
    public class MapModel 
    {
        


        public event PropertyChangedEventHandler PropertyChanged;
        TcpClient telnetClient; //Need to change to TcpClient
        private NetworkStream stream;
        private BinaryReader reader;
        volatile Boolean stop;
        public MapModel(TcpClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.stop = false;
            this.connect("127.0.0.1", 5402);
            this.start();
        }

               
        private double latitude_deg;
        private double longtitude_deg;

        public double Latitude_deg { get { return latitude_deg; } set { latitude_deg = value; NotifyPropertyChanged("latitude_deg"); } }
        public double Longtitude_deg { get { return longtitude_deg; } set { longtitude_deg = value; NotifyPropertyChanged("longtitude_deg"); } }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

        }


        public void connect(string ip, int port)
        {
            this.telnetClient.Connect(ip, port);
            Console.WriteLine("CONNECTED)?");
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
                { 
                    this.write("get /position/latitude-deg");
                    Latitude_deg = Double.Parse(read(telnetClient));
                    Console.WriteLine("Received: " + latitude_deg);
                    Console.WriteLine("Property after UPDATE: " + latitude_deg);

                    this.write("get /position/longitude-deg");
                    Longtitude_deg = Double.Parse(read(telnetClient));

                    Thread.Sleep(250);
                }

            }).Start();
        }
    }
}

