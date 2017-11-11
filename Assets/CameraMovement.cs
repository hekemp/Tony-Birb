using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public GameObject birb;

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - birb.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, birb.transform.position.x + offset.x, .1f), Mathf.Lerp(this.transform.position.y, birb.transform.position.y + offset.y, .1f) , this.transform.position.z);
		//this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, birb.transform.position.x, .5f), this.transform.position.y, this.transform.position.z);

	}
}
