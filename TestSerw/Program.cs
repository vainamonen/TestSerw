using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestSerw
{
    class Program
    {
        NetworkStream nstream;
        bool isConnected;

        public Program(NetworkStream nstream2)
        {
            this.nstream = nstream2;
            isConnected = true;
        }

        static void Main(string[] args)
        {
            int sPort = 2000;
            IPAddress ip = IPAddress.Parse("127.0.0.1");     //adres serwera
            TcpListener serwer = new TcpListener(ip, sPort);  //tworzymy obietk serwer
            //na słuchujący podanym porcie
            Console.WriteLine("Oczekiwanie na klienta");
            serwer.Start();                      //uruchamiamy serwer

            TcpClient client = serwer.AcceptTcpClient(); //akceptujemy żądanie połączenia
            
            NetworkStream nstream = client.GetStream();  //pobieramy strumień do wymiany danych

            Program p = new Program(nstream);

            new Thread(p.reader).Start();
            new Thread(p.writer).Start();
            Console.WriteLine("Rozpoczęto rozmowę z klientem");


        }
        private void reader()
        {
            StreamReader streamr = new StreamReader(nstream);
            String odp;
            while (true)
            {
                odp = streamr.ReadLine();
                Console.WriteLine("Odczytano: " + odp);
               
            }
        }

        //Funkcja przesyłająca dane do serwera
        //Wykonywana w osobnym watku
        private void writer()
        {
            StreamWriter streamw = new StreamWriter(nstream);
            while (true)
            {
                String myString = Console.ReadLine();
                streamw.WriteLine(myString);
                streamw.Flush();
            }
        }
    }
}
