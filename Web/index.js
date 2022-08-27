const btnConnect = document.getElementById("btnConnect");
const peerInput = document.getElementById("peerInput");
const roomInput = document.getElementById("roomInput");

const videoContainer = document.getElementById("videoContainer");
var signalingServer = "https://rtc.medialooks.com:8889";

btnConnect.addEventListener("click", connect);

function connect() {
  var peerId = peerInput.value == "" ? "Streamer471" : peerInput.value;
  var room = roomInput.value == "" ? "Room660" : roomInput.value;
  var webrtc = new SimpleWebRTC({
    target: peerId,
    url: signalingServer,
    iceServers: [
      { urls: "stun:stun.l.google.com:19302" },
      {
        username: "test_user",
        credential: "medialooks",
        urls: ["turn:67.220.183.67:3478"],
      },
    ],
    localVideoEl: "",
    remoteVideosEl: "",
    autoRequestMedia: false,
    debug: true,
    detectSpeakingEvents: true,
    autoAdjustMic: false,
    media: {
      video: true,
      audio: false,
    },
  });

  /*   
Webrtc nepríjme call pravdepodobne kvôli validácii na strane servera


webrtc.on("readyToCall", function () {
    webrtc.setInfo("", webrtc.connection.connection.id, ""); // Store strongId

    if (room) {
      webrtc.joinRoom(room);
    }
  }); 

*/
  if (room) {
    webrtc.joinRoom(room);
  }

  webrtc.on("videoAdded", function (video, peer) {
    console.log("video added", peer);
    var container = document.getElementById("videoContainer");
    video.setAttribute("loop", "");
    video.setAttribute("autoplay", "true");
    video.setAttribute("controls", "");
    video.setAttribute("width", "100%");
    video.setAttribute("height", "100%");

    videoEl = video;
    container.innerHTML = "";
    container.appendChild(video);
    webrtc.stopLocalVideo();
    video.muted = true;
    video.play();
  });

  //Handle removing video by target peer
  webrtc.on("videoRemoved", function (video, peer) {
    console.log("video removed ", peer);
    var container = document.getElementById("videoContainer");
    if (
      peer.id == peerId ||
      peer.strongId == peerId ||
      peer.nickName == peerId
    ) {
      videoEl = null;
      container.innerHTML = "";
      var videoStub = document.createElement("video");
      container.appendChild(videoStub);
    }
  });
}
