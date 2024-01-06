






const constraints = { "video": { width: { max: 320 } }, "audio": true };



var theStream;
var theRecorder;
var recordedChunks = [];



function startFunction() {
    navigator.mediaDevices.getUserMedia(constraints)
        .then(gotMedia)
        .catch(e => { console.error('getUserMedia() failed: ' + e); });
}



function gotMedia(stream) {
    theStream = stream;
    var video = document.querySelector('video');
    video.srcObject = stream;
    try {
        recorder = new MediaRecorder(stream, { mimeType: "video/webm" });
    } catch (e) {
        console.error('Exception while creating MediaRecorder: ' + e);
        return;
    }

    theRecorder = recorder;
    recorder.ondataavailable =
        (event) => { recordedChunks.push(event.data); };
    recorder.start(100);
}



// From @samdutton's "Record Audio and Video with MediaRecorder"
// https://developers.google.com/web/updates/2016/01/mediarecorder
function download() {
    theRecorder.stop();
    theStream.getTracks().forEach(track => { track.stop(); });



    var blob = new Blob(recordedChunks, { type: "video/webm" });
    var url = URL.createObjectURL(blob);
    var a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";
    a.href = url;

    var form = new FormData();
    form.append("filename", "test");
    form.append("blob", blob);

   
    //$.ajax({
    //    type: "POST",
    //    url: "/Home/PostRecordedAudioVideo",
    //    data: JSON.stringify(form),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (response) {
    //        if (response != null) {
    //            alert("Name : " + response.Name + ", Designation : " + response.Designation + ", Location :" + response.Location);
    //        } else {
    //            alert("Something went wrong");
    //        }
    //    },
    //    failure: function (response) {
    //        alert(response.responseText);
    //    },
    //    error: function (response) {
    //        alert(response.responseText);
    //    }
    //});  

    a.download = 'test.webm';
    a.click();
    // setTimeout() here is needed for Firefox.
    setTimeout(function () { URL.revokeObjectURL(url); }, 100);
}