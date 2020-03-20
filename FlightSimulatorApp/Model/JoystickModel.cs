/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FlightSimulatorApp.Model
{
    public class JoystickModel 
    {
        


        public event PropertyChangedEventHandler PropertyChanged;
        TcpClient telnetClient; //Need to change to TcpClient
        private NetworkStream stream;
        private BinaryReader reader;
        volatile Boolean stop;
        public JoystickModel(TcpClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.stop = false;
            this.connect("127.0.0.1", 5402);
            this.start();
        }

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

        public void move(double rudder, double aileron, double throttle, double elevator)
        {
            this.write("set /controls/flight/rudder "+rudder+"\n"); //Need full path
            Console.WriteLine("Sent: "+rudder);


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

        
    }
}

*/