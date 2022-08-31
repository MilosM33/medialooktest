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


        static ZInputFile input1 = new ZInputFile("video1.mp4");
        static ZInputFile input2 = new ZInputFile("video2.mp4");


        const string ROOM_URL = "http://rtc.medialooks.com:8889/Room9999/";

        static ZInputStream outputStream = new ZInputStream(ROOM_URL, streamer_name: "Output");
        static ZInputStream inputStream1 = new ZInputStream(ROOM_URL, streamer_name: "Input1");
        static ZInputStream inputStream2 = new ZInputStream(ROOM_URL, streamer_name: "Input2");

        static void Main(string[] args)
        {
            // Add inputs to stream

            outputStream.AddVideoSource(input1.GetSource());
            inputStream1.AddVideoSource(input1.GetSource());
            inputStream2.AddVideoSource(input2.GetSource());
            
            // Communication layer

            ZCommunicationStream communicationStream = new ZCommunicationStream(outputStream);
            communicationStream._webrtc.OnEventSafe += onMessageReceived;

            Console.ReadLine();
          
        }
        private static void onMessageReceived(string bsChannelID, string bsEventName, string bsEventParam, object pEventObject)
        {
            int value;
            if (bsEventName == "message" && int.TryParse(bsEventParam, out value))
            {
                
                Console.WriteLine($"Zmena streamu na stream {value} ");
                switch (value)
                {
                    case 1:
                        outputStream.ChangeVideoSource(input1.GetSource());
                        break;
                    case 2:
                        outputStream.ChangeVideoSource(input2.GetSource());
                        break;
                    default:
                        break;
                }
            }

        }


    }
}
