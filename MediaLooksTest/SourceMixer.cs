using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class SourceMixer
    {
        private MMixerClass _mixer;
        public SourceMixer()
        {
            _mixer = new MMixerClass();
            _mixer.ObjectStart(null);
            
        }

        public void AddStream(String streamID, String filePath)
        {
            _mixer.StreamsAdd(streamID, null, filePath, "", out _, 1.00);
        }

        public void AddStreamToScene(String streamID)
        {
            MElement root;
            _mixer.ElementsGetByIndex(0, out root);
            (root as IMElements).ElementsAdd("", "video", "stream_id=" + streamID + " h=0.5 w=0.5 show=1 audio_gain=0", out _, 0);
            _mixer.FilePlayStart();

        }

        public void Play(MWebRTCClass webRTC)
        {   
            _mixer.PluginsAdd(webRTC, 10);
            

        }
    }
}
