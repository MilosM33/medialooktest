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



        const string ROOM_URL = "http://rtc.medialooks.com:8889/Room9999/";
        static void Main(string[] args)
        {
            Console.WriteLine(ROOM_URL);

            string videoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Videos");
 
            // Inputs and outputs

            ZInput input1 = new ZInput(Path.Combine(videoPath, "video1.mp4"));
            ZInput input2 = new ZInput(Path.Combine(videoPath, "video2.mp4"));
            ZInput input3 = new ZInput(Path.Combine(videoPath, "video3.mp4"));

            ZOutput output = new ZOutput(Path.Combine(videoPath, "video1.mp4"));


            // Output stream
            ZOutputStream stream1 = new ZOutputStream(ROOM_URL, streamer_name:"Output");
            stream1.Init();
            stream1.AddVideoSource(output.GetSource());


            // Previews of input streams
            ZInputStream stream2 = new ZInputStream(ROOM_URL, streamer_name: "Input1");
            stream2.Init();
            stream2.AddVideoSource(input1.GetSource());

            ZInputStream stream3 = new ZInputStream(ROOM_URL, streamer_name: "Input2");
            stream3.Init();
            stream3.AddVideoSource(input2.GetSource());

            ZInputStream stream4 = new ZInputStream(ROOM_URL, streamer_name: "Input3");
            stream4.Init();
            stream4.AddVideoSource(input3.GetSource());

            // Communication layer

            List<ZInput> inputs = new List<ZInput>()
            {
                input1, input2, input3
            };

            // Output stream webrtc
            MWebRTCClass webrtc = stream1.GetWebRTC();

            ZCommunicationStream communicationStream = new ZCommunicationStream(webrtc);
            communicationStream.SetInputs(inputs);

            communicationStream.SetOutputStream(stream1);
            communicationStream.Init();


            Console.ReadLine();
          
        }

        
    }
}
