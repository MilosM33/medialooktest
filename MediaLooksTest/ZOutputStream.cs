using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class ZOutputStream
    {
        private MWebRTCClass _webrtc;
        private String _url;
        private MFileClass _source;
        public ZOutputStream(String room_url, String streamer_name)
        {
            _webrtc = new MWebRTCClass();
            _url = room_url + streamer_name;

        }

        public void Init()
        {
            // nastavíme iba odosielanie videa, inak posielame aj camera stream ( video konferencia)
            _webrtc.PropsSet("mode", "sender");

            _webrtc.Login(_url, "", out _);

        }

        public void AddVideoSource(MFileClass source)
        {
            _source = source;
            _source.PluginsAdd(_webrtc, 10);
        }
        public void ChangeVideoSource(String path, double position)
        {
            _source.FileNameSet(path, "");
            _source.FilePosSet(position, 0);
        }
        public MWebRTCClass GetWebRTC()
        {
            return _webrtc;
        }
    }
}
