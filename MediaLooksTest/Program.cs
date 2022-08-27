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
        static List<String> videoPaths = new List<string>();


        static MWebRTCClass masterRTC = new MWebRTCClass();
        static List<MWebRTCClass> slaves = new List<MWebRTCClass>();
        static void Main(string[] args)
        {
            
            
            SourceMixer sourceMixer = new SourceMixer();
            String url = GenerateRandomUrl();

            
            
            String videoPath = @"C:\Users\milos\programy\Programovanie\Wezeo\MediaLooksTest\MediaLooksTest\Videos";

            foreach (String filePath in Directory.EnumerateFiles(videoPath))
            {
                MFileClass temp = new MFileClass();
                temp.FileNameSet(filePath, "");
                temp.PropsSet("loop", "true");
                videos.Add(temp);
                videoPaths.Add(filePath);

                 MWebRTCClass slave = new MWebRTCClass();
                 slave.Login(url, "", out _);
                 slaves.Add(slave);
                
            }
            // Choose master
            masterRTC = slaves[0];
            masterRTC.OnEvent += MasterRTC_OnEvent;
            // Stream
            // Half duplex iba video stream, žiadna webka
            masterRTC.PropsSet("mode", "sender");
            current = videos[0];
            Console.WriteLine(url);

            for (int i = 0; i < videos.Count; i++)
            {
                MFileClass file = videos[i];
                MWebRTCClass slave = slaves[i];

                file.FilePlayStart();
                file.PluginsAdd(slave, 10);

            }

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
                    MFileClass prev = current;
                    current = videos[value];

                    String prevPath;
                    String currentPath;

                    current.FileNameGet(out currentPath);
                    prev.FileNameGet(out prevPath);

                    current.FileNameSet(prevPath, "");
                    prev.FileNameSet(currentPath, "");





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
