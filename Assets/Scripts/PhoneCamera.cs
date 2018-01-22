using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {

	private bool mIsCameraAvailable;
	private WebCamTexture mBackCamera;
	private Texture mDefaultBackground;

	public RawImage background;
	public AspectRatioFitter aspectRatioFitter;

	void Start () {
		mDefaultBackground = background.texture;		

		WebCamDevice[] cameraDevices = WebCamTexture.devices;
		if (cameraDevices.Length == 0) {
			Debug.Log ("No camera devices detected");
			mIsCameraAvailable = false;
			return;
		}

		for (int i = 0; i < cameraDevices.Length; i++) {
			if (!cameraDevices [i].isFrontFacing) {
				mBackCamera = new WebCamTexture (cameraDevices[i].name, Screen.width, Screen.height);
				break;
			}
		}

		if (mBackCamera == null) {
			Debug.Log ("No back camera detected");
			return;
		}

		mBackCamera.Play ();
		background.texture = mBackCamera;

		mIsCameraAvailable = true;
	}
	
	void Update () {
		if (!mIsCameraAvailable) {
			return;
		}

		float aspectRatio = (float)mBackCamera.width / mBackCamera.height;
		aspectRatioFitter.aspectRatio = aspectRatio;

		float scaleY = mBackCamera.videoVerticallyMirrored ? -1 : 1;
		background.rectTransform.localScale = new Vector3 (1, scaleY, 1);

		int orient = -mBackCamera.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3 (0, 0, orient);
	}
}
