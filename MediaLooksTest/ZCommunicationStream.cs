using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class ZCommunicationStream
    {
        public MWebRTCClass _webrtc;
        public ZCommunicationStream(ZInputStream stream)
        {
            _webrtc = stream.GetWebRTC();
        }
    }
}
