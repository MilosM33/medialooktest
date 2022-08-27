using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class Program
    {




        static void Main(string[] args)
        {
            MWriterClass writer = new MWriterClass();
            MWebRTCClass webRTC = new MWebRTCClass();
            Random r = new Random();
            const string signalingStr = "http://rtc.medialooks.com:8889";
            String url = signalingStr + $"/Room{r.Next(1000)}/Streamer{r.Next(1000)}";
            String pbsId = "";

            List<MFileClass> videos = new List<MFileClass>();
            String videoPath = @"C:\Users\milos\programy\Programovanie\Wezeo\MediaLooksTest\MediaLooksTest\Videos";
            foreach(String filePath in Directory.EnumerateFiles(videoPath))
            {
                MFileClass temp = new MFileClass();
                temp.FileNameSet(filePath,"");
                videos.Add(temp);
            }

            // Stream
            // Half duplex iba video stream, žiadna webka
            webRTC.PropsSet("mode", "sender");

            webRTC.Login(url, "", out pbsId);

            Console.WriteLine(url);

            MFileClass current = videos[0];
            current.PropsSet("loop", "true");
            current.FilePlayStart();
            current.PluginsAdd(webRTC, 10);
            while (true)
            {
                Console.WriteLine("Zadaj číslo od 1 po " + videos.Count);
                int input = 0;
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    
                    if(input < videos.Count + 1)
                    {
                        //Zastaví video
                        current.PropsSet("loop", "false");
                        current.FilePlayStop(0);
                        current.PluginsRemove(webRTC);

                        // Zmení video
                        current = videos[input - 1];
                        current.PropsSet("loop", "true");
                        current.FilePlayStart();
                        current.PluginsAdd(webRTC,10);


                    }
                }
                
            }
            Console.ReadLine();
            webRTC.Logout();
        }
    }
}
