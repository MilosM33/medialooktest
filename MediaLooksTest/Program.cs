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



        static MFileClass current;
        static List<MFileClass> videos = new List<MFileClass>();
        static MWebRTCClass masterRTC = new MWebRTCClass();
        static void Main(string[] args)
        {
            
            
            SourceMixer sourceMixer = new SourceMixer();
            String url = GenerateRandomUrl();

            
            List<MWebRTCClass> slaves = new List<MWebRTCClass>();
            String videoPath = @"C:\Users\milos\programy\Programovanie\Wezeo\MediaLooksTest\MediaLooksTest\Videos";

            // Stream
            // Half duplex iba video stream, žiadna webka
            masterRTC.PropsSet("mode", "sender");

            masterRTC.Login(url, "", out _);
            masterRTC.OnEvent += MasterRTC_OnEvent;



            slaves.Add(masterRTC);

            foreach (String filePath in Directory.EnumerateFiles(videoPath))
            {
                MFileClass temp = new MFileClass();
                temp.FileNameSet(filePath, "");
                temp.PropsSet("loop", "true");
                videos.Add(temp);

                /*
                 MWebRTCClass slave = new MWebRTCClass();
                slave.Login(url, "", out _);
                slaves.Add(slave);
                */
            }
            current = videos[0];
            
            current.FilePlayStart();
            current.PluginsAdd(masterRTC,10);
            Console.WriteLine(url);

            /*
             * 
             * Multiple 
                for (int i = 0; i < videos.Count; i++)
            {
                MFileClass file = videos[i];
                MWebRTCClass slave = slaves[i];

                file.PropsSet("loop", "false");
                file.FilePlayStop(0);
                file.PluginsAdd(slave, 10);

            }
             */

            Console.ReadLine();
            masterRTC.Logout();
        }

        private static void MasterRTC_OnEvent(string bsChannelID, string bsEventName, string bsEventParam, object pEventObject)
        {
            int value;
            if(bsEventName == "message" && int.TryParse(bsEventParam, out value))
            {
                Console.WriteLine($"Zmena streamu na stream {value} ");
                value -= 1;
                if (value < videos.Count)
                {
                    current.FilePlayStop(0);
                    current.PluginsRemove(masterRTC);
                    current = videos[value];
                    current.PluginsAdd(masterRTC,10);
                    current.FilePlayStart();
                }
                else
                {
                    Console.WriteLine("Zadajte číslo od 1 po " + videos.Count);
                }
            }
        }

        const string SIGNALINGSERVER = "http://rtc.medialooks.com:8889";
        static Random r = new Random();
        static string GenerateRandomRoom()
        {
            String url = SIGNALINGSERVER + $"/Room{r.Next(1000)}";
            return url;
        }
        static string GenerateRandomUrl()
        {
            String url = GenerateRandomRoom() + $"/Streamer{r.Next(1000)}";
            return url;
        }
    }
}
