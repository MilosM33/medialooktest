using System;
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
            MSendersClass mSendersClass = new MSendersClass();
            MFileClass mFile = new MFileClass();
            Random r = new Random();

            const string signalingStr = "https://rtc.medialooks.com:8889";

            String url = (signalingStr + "/Room" + +r.Next(1000) + "/Streamer" + r.Next(1000));
            String pbsId = "";
            mFile.FileNameSet(@"C:\Users\milos\programy\Programovanie\Wezeo\MediaLooksTest\MediaLooksTest\Videos\video1.mp4", "");
            mFile.PropsSet("loop", "true");
            mFile.FilePlayStart();
            mFile.PluginsAdd(webRTC, 10);



            // Stream
            // Half duplex
            webRTC.PropsSet("mode", "sender");
            webRTC.Login(url, "", out pbsId);
            

            Console.WriteLine(url);

            Console.ReadLine();
            webRTC.Logout();
        }
    }
}
