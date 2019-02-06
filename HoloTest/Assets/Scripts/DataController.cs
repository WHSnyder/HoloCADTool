using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.Networking;


public class DataController : MonoBehaviour {

    GameObject ball = null;
    KeywordRecognizer recog;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    private byte[] data;

    PhotoCapture copped = null;
    Texture2D targ = null;
    CameraParameters cameraParameters;
    Resolution cameraResolution;

    Rect frame;

    GameObject s;

    //data control vars
    bool stop = false;
    string currentGesture = "";

    private void Start(){

        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Last();
        targ = new Texture2D(cameraResolution.width, cameraResolution.height, TextureFormat.BGRA32, false);
        frame = new Rect(0, 0, 128, 128);
    }


    private void capture(){

        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            copped = captureObject;
            cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            copped.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result){
                // Take a picture
                copped.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }



    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame){

        // Copy the raw image data into the target texture
        photoCaptureFrame.UploadImageDataToTexture(targ);

        harvest(targ);

        // Deactivate the camera
        copped.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result){

        // Shutdown the photo capture resource
        copped.Dispose();
        copped = null;
    }






    private void Awake(){

        keywords.Add("go", () => { stop = false;  capture(); stop = true; });
        keywords.Add("stop", () => { stop = true; });

        keywords.Add("off", () => { stop = false; sendTrainingData('off'); stop = true; });

        //keywords.Add("test", ) 

        recog = new KeywordRecognizer(keywords.Keys.ToArray());
        recog.OnPhraseRecognized += Act;
        recog.Start();
    }


    private void Act(PhraseRecognizedEventArgs args){
        System.Action action;

        if (keywords.TryGetValue(args.text, out action)){
            action.Invoke();
        }
    }


    void harvest(Texture2D buffer){
        data = buffer.GetRawTextureData();
        StartCoroutine(sendImage(data));
    }


    IEnumerator sendImage(byte[] myData){

        Dictionary<string, string> headers = new Dictionary<string, string>();

        UnityWebRequest www = UnityWebRequest.Put("http://10.105.139.204:8090/", myData);
        www.SetRequestHeader("width", cameraResolution.width.ToString());
        www.SetRequestHeader("height", cameraResolution.height.ToString());
        www.SetRequestHeader("type", currentGesture);

        www.chunkedTransfer = false;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError){
            Debug.Log(www.error);
        }
        else{
            Debug.Log("Upload complete!");
        }
    }


    IEnumerator sendTrainingData(string type){
        currentGesture = type;
        while (!stop){ 
            capture();
            yield return null;
        }
        currentGesture = "";
    }
}
