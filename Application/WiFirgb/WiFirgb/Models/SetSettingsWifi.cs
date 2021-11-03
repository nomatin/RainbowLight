using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace wifirgb.Models
{
    static class SetSettingsWifi
    {
        static private string history = "";
        static private IPAddress ip = IPAddress.Parse("226.1.1.1");
        static private int port = 0;
        static private int Name = 0;
        static private int Group = 0;
        static private int ListenPort = 40903;
        static private int timeOut = 10000;
        static private bool search = true;
        static public UdpClient sender = new UdpClient(); // создаем UdpClient для отправки
        static private IPEndPoint endPoint = new IPEndPoint(ip, port);
        static public int group 
        { 
            set
            {
                if (value >= 0)
                {

                    Group = value;
                }
            }
            get
            {
                return Group;
            } 
        }
        static public int name
        {
            set
            {
                if (value >= 0)
                {
                    Name = value;
                }
            }
            get
            {
                return Name;
            }
        }
        static public bool SetPort(int Port)
        {
            if(Port>=1024 || Port <= 49151)
            {
                port = Port;
                endPoint = new IPEndPoint(ip, port);
                return true;
            }
            else
            {
                return false;
            }
        }
        static public bool SetIp(string Ip)
        {
            try
            {
                ip = IPAddress.Parse(Ip);
                endPoint = new IPEndPoint(ip, port);
                return true;

            }
            catch
            {
                return false;
            }
        }
        static public void setAP(string nameAP)
        {
            string command = "0:5:1:" + nameAP;
            sendCommand(command);
        }
        static public void setWiFi(string name, string pass)
        {
            string command = "0:4:";
            command += name + ":" + pass;
            sendCommand(command);
        }
        static public void setBrightness(int brightness)
        {
            string command = "0:0:";
            command += brightness.ToString();
            sendCommand(command);
        }
        static public void setSpeed(int speed)
        {
            string command = "0:1:";
            command += speed.ToString();
            sendCommand(command);
        }
        static public void setRGB(int r, int g, int b)
        {
            string command = "1:0:";
            command += r.ToString() + ":" + g.ToString() + ":" + b.ToString();
            sendCommand(command);
        }
        static public void setW(int W)
        {
            string command = "1:6:";
            command += W.ToString();
            sendCommand(command);
        }
        
        static public void setKelvin(int temp)
        {
            string command = "1:2:";
            command += temp.ToString();
            sendCommand(command);
        }
        static public void setColor(int color)
        {
            string command = "1:2:";
            command += color.ToString();
            sendCommand(command);
        }
        static public void setStrobe(int speed)
        {
            string command = "1:5:";
            command += speed.ToString();
            sendCommand(command);

        }
        static public void setFire(int color, int speed, double Ratio)
        {
            string command = "1:4:";
            command += color.ToString() + ":" + speed.ToString() + ":" + (int)Ratio;
            sendCommand(command);
        }
        static public void setRain(int numRain, int speed)
        {
            string command = "1:3:";
            command += numRain.ToString() + ":" + speed.ToString();
            sendCommand(command);
        }
        static public void Power()
        {
            setBrightness(0);
        }
        static public void StartSearch(Delegate eventFuncshen)
        {
            search = true;
            
            Timer aTimer = new Timer(10000);
            aTimer.Elapsed += sendSearch;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            Thread thread = new Thread(() => requestName(eventFuncshen));
            thread.Start();

        }
        static private void sendSearch(Object source, ElapsedEventArgs e)
        {
            if (search)
            {
                string command = "0:6:" + ListenPort.ToString();
                sendCommand(command);
            }
            


        }
        static private void requestName(Delegate eventFuncshen)
        {
            UdpClient receiver = new UdpClient(ListenPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null;
            List<string> answer = new List<string>();
            Console.WriteLine("Поиск");
            while (search)
            {
                
                byte[] data = receiver.Receive(ref remoteIp);
                eventFuncshen.DynamicInvoke(Encoding.ASCII.GetString(data));
                
            }
            Console.WriteLine("Стоп");

        }
        static public void StopSearch()
        {
            search = false;
        }
        static public bool startServer()
        {
            try
            {
                sender = new UdpClient(); // создаем UdpClient для отправки
                endPoint = new IPEndPoint(ip, port);
                return true;
            }
            catch
            {
                return false;
            }
        }
        static public bool closeServer()
        {
            try
            {
                sender.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        static private void sendCommand(string message)
        {
            message = Name.ToString() + ":" + Group.ToString() + ":" + message;
            if(message != history)
            {
                history = message;
                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    sender.Send(data, data.Length, endPoint); // отправка
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            
        }
    }
    
}
