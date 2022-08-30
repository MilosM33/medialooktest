// Setup

const btnConnect = document.getElementById("btnConnect");
const btnSwitch = document.getElementById("btnSwitch");
const cameraNumber = document.getElementById("cameraNumber");
const peerInput = document.getElementById("peerInput");
const roomInput = document.getElementById("roomInput");

const inputContainer = document.getElementById("inputContainer");
const outputContainer = document.getElementById("outputContainer");
let masterId = "";
const signalingServer = "http://rtc.medialooks.com:8889";

let output;

btnConnect.addEventListener("click", connect);
btnSwitch.addEventListener("click", switchCamera);

function connectToPeer(peerId) {
  webrtc = new SimpleWebRTC({
    target: peerId,
    url: "http://rtc.medialooks.com:8889/",
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

  webrtc.joinRoom("Room9999");
  return webrtc;
}

function onAddVideo(webrtc, containerId) {
  webrtc.on("videoAdded", function (video, peer) {
    console.log("video added", peer);
    const container = document.getElementById(containerId);

    video.setAttribute("loop", "");
    video.setAttribute("autoplay", "true");
    video.setAttribute("muted", "true");
    video.setAttribute("controls", "");
    video.setAttribute("width", "480px");

    container.innerHTML = "";
    container.appendChild(video);
    webrtc.stopLocalVideo();
    video.muted = true;
    video.play();
  });
}

function onRemoveVideo(webrtc, containerId) {
  webrtc.on("videoRemoved", function (video, peer) {
    console.log("video removed", peer);
    const container = document.getElementById(containerId);
    container.innerHTML = "";
  });
}

// Controls

function switchCamera() {
  let value = cameraNumber.value;
  if (value == "") {
    value = 0;
  }
  output.sendDataChannelMessageToPeer("Output", value);
}

function connect() {
  output = connectToPeer("Output");
  onAddVideo(output, "outputContainer");
  onRemoveVideo(output, "outputContainer");

  const input1 = connectToPeer("Input1");
  onAddVideo(input1, "inputContainer1");
  onRemoveVideo(input1, "inputContainer1");

  const input2 = connectToPeer("Input2");
  onAddVideo(input2, "inputContainer2");
  onRemoveVideo(input2, "inputContainer2");

  const input3 = connectToPeer("Input3");
  onAddVideo(input3, "inputContainer3");
  onRemoveVideo(input3, "inputContainer3");
}
