using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPLATFORMLib;
namespace MediaLooksTest
{
    internal class InputStream
    {
        private MWebRTCClass _webrtc;
        private String _url;
        public InputStream(String room_url, String streamer_name)
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
            source.PluginsAdd(_webrtc, 10);
        }
    }
}
