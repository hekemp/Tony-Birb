using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	public static float ScreenShakeIntensity = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ScreenShakeIntensity > 0) {
			float oldZ = Camera.main.transform.localPosition.z;
			Camera.main.transform.localPosition = new Vector3 (Random.Range (-1, 1) * ScreenShakeIntensity, Random.Range (-1, 1) * ScreenShakeIntensity, oldZ);
		}
	}
}
