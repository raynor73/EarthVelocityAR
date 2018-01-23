using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {

	private Vector3 DOWN = new Vector3(0, -1, 0);
	private WebCamTexture mBackCamera;
	private bool mIsArReady = false;

	public RawImage background;
	public AspectRatioFitter aspectRatioFitter;
	public Transform skyTransform;

	void Start () {
		if (!SystemInfo.supportsAccelerometer) {
			Debug.Log ("No accelerometer detected");
			return;
		}

		WebCamDevice[] cameraDevices = WebCamTexture.devices;
		if (cameraDevices.Length == 0) {
			Debug.Log ("No camera devices detected");
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

		mIsArReady = true;
	}
	
	void Update () {
		if (!mIsArReady) {
			return;
		}

		float aspectRatio = (float)mBackCamera.width / mBackCamera.height;
		aspectRatioFitter.aspectRatio = aspectRatio;

		float scaleY = mBackCamera.videoVerticallyMirrored ? -1 : 1;
		background.rectTransform.localScale = new Vector3 (1, scaleY, 1);

		int orient = -mBackCamera.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3 (0, 0, orient);

		Vector3 accelerationDirectionVector = Vector3.zero;
		accelerationDirectionVector = Input.acceleration;
		accelerationDirectionVector.z *= -1;
		accelerationDirectionVector.Normalize ();
		skyTransform.localRotation = Quaternion.FromToRotation (DOWN, accelerationDirectionVector);

		//debug.localPosition = accelerationDirectionVector;

		//skyTransform.localRotation = Quaternion.Inverse(GyroToUnity(Input.gyro.attitude));
	}

	/*private static Quaternion GyroToUnity(Quaternion q) {
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}*/
}
