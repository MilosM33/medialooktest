using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class CommunicationStream
    {
        private MWebRTCClass _webrtc;
        private List<Input> _inputs;
        private OutputStream _outputStream;
        public CommunicationStream(MWebRTCClass webrtc)
        {
            _webrtc = webrtc;
        }

        public void SetInputs(List<Input> inputSources)
        {
            _inputs = inputSources;
        }
        public void SetOutputStream(OutputStream outputStream)
        {
            _outputStream = outputStream;
        }
        public void Init()
        {
            _webrtc.OnEventSafe += onMessageReceived;
        }

        private void onMessageReceived(string bsChannelID, string bsEventName, string bsEventParam, object pEventObject)
        {
            int value;
            if (bsEventName == "message" && int.TryParse(bsEventParam, out value))
            {
                value--;
                Console.WriteLine($"Zmena streamu na stream {value} ");
                if (value < _inputs.Count)
                {
                    String videoPath;
                    MFileClass next = _inputs[value].GetSource();
                    double position;


                    // get video path
                    next.FileNameGet(out videoPath);

                    // get video playback time
                    next.FilePosGet(out position);

                    
                    _outputStream.ChangeVideoSource(videoPath, position);
                    
                }
                else
                {
                    Console.WriteLine("Zadajte číslo od 1 po " + _inputs.Count);
                }
            }
            
        }
    }
}
