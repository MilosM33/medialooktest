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

            Input input1 = new Input(Path.Combine(videoPath, "video1.mp4"));
            Input input2 = new Input(Path.Combine(videoPath, "video2.mp4"));
            Input input3 = new Input(Path.Combine(videoPath, "video3.mp4"));

            Output output = new Output(Path.Combine(videoPath, "video1.mp4"));


            // Output stream
            OutputStream stream1 = new OutputStream(ROOM_URL, "Output");
            stream1.Init();
            stream1.AddVideoSource(output.GetSource());


            // Previews of input streams
            InputStream stream2 = new InputStream(ROOM_URL, "Input1");
            stream2.Init();
            stream2.AddVideoSource(input1.GetSource());

            InputStream stream3 = new InputStream(ROOM_URL, "Input2");
            stream3.Init();
            stream3.AddVideoSource(input2.GetSource());

            InputStream stream4 = new InputStream(ROOM_URL, "Input3");
            stream4.Init();
            stream4.AddVideoSource(input3.GetSource());

            // Communication layer

            List<Input> inputs = new List<Input>()
            {
                input1, input2, input3
            };

            MWebRTCClass webrtc = stream1.GetWebRTC();
            CommunicationStream communicationStream = new CommunicationStream(webrtc);
            communicationStream.SetInputs(inputs);

            communicationStream.SetOutputStream(stream1);
            communicationStream.Init();


            Console.ReadLine();
          
        }

        
    }
}
