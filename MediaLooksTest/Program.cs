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



        static MFileClass masterSource;
        static List<MFileClass> videos = new List<MFileClass>();
        static List<String> videoPaths = new List<string>();
        static String videoPath = @"C:\Users\milos\programy\Programovanie\Wezeo\MediaLooksTest\MediaLooksTest\Videos";

        static MWebRTCClass masterRTC = new MWebRTCClass();
        static List<MWebRTCClass> slaves = new List<MWebRTCClass>();
        static void Main(string[] args)
        {
            String url = GenerateRandomUrl();

            masterRTC = new MWebRTCClass();
            masterRTC.Login(url, "", out _);

            // create slaves
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

                temp.FilePlayStart();
                temp.PluginsAdd(slave, 10);

            }

            masterSource = new MFileClass();
            masterSource.FileNameSet(videoPaths[0], "");
            masterSource.PropsSet("loop", "true");
            masterSource.FilePlayStart();
            masterSource.PluginsAdd(masterRTC, 10);

            masterRTC.OnEvent += MasterRTC_OnEvent;

            // Stream
            // Half duplex iba video stream, žiadna webka
            masterRTC.PropsSet("mode", "sender");
            Console.WriteLine(url);


            Console.ReadLine();
            masterRTC.Logout();
        }




        private static void MasterRTC_OnEvent(string bsChannelID, string bsEventName, string bsEventParam, object pEventObject)
        {
            int value;
            if (bsEventName == "message" && int.TryParse(bsEventParam, out value))
            {
                value--;
                Console.WriteLine($"Zmena streamu na stream {value} ");
                if (value < videos.Count)
                {
                    MFileClass next = videos[value];
                    double position;
                    // get video playback time
                    next.FilePosGet(out position);

                    masterSource.FileNameSet(videoPaths[value], "");
                    masterSource.FilePosSet(position, 0);
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
