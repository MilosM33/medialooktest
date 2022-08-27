const btnConnect = document.getElementById("btnConnect");
const btnSwitch = document.getElementById("btnSwitch");
const cameraNumber = document.getElementById("cameraNumber");
const peerInput = document.getElementById("peerInput");
const roomInput = document.getElementById("roomInput");

const videoContainer = document.getElementById("videoContainer");
const masterContainer = document.getElementById("masterContainer");
let masterId = "";
const signalingServer = "http://rtc.medialooks.com:8889";

let webrtc;
let peerId;
let room;

btnConnect.addEventListener("click", connect);
btnSwitch.addEventListener("click", switchCamera);

function switchCamera() {
  let value = cameraNumber.value;
  if (value == "") {
    value = 0;
  }
  webrtc.sendDataChannelMessageToPeer(peerId, value);
}
function addVideo(video) {
  const parrent = document.createElement("div");
  parrent.className = "col-md-4";
  parrent.appendChild(video);
  return parrent;
}

function connect() {
  peerId = peerInput.value == "" ? "Streamer983" : peerInput.value;
  room = roomInput.value == "" ? "Room505" : roomInput.value;
  webrtc = new SimpleWebRTC({
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
    debug: false,
    detectSpeakingEvents: true,
    autoAdjustMic: false,
    media: {
      video: true,
      audio: false,
    },
  });

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

    // ak master je prazdny
    if (masterContainer.childElementCount === 0) {
      masterContainer.appendChild(video);
      masterId = video.id;
    } else {
      container.appendChild(addVideo(video));
    }

    webrtc.stopLocalVideo();
    video.muted = true;
    video.play();
  });

  webrtc.on("videoRemoved", function (video, peer) {
    console.log("video removed ", peer);
    var video = document.getElementById(video.id).parentNode;
    if (masterId === video.id) {
      masterContainer.removeChild(video);
    } else {
      videoContainer.removeChild(video);
    }

    if (
      peer.id == peerId ||
      peer.strongId == peerId ||
      peer.nickName == peerId
    ) {
      var videoStub = document.createElement("video");
      container.appendChild(videoStub);
    }
  });
}
